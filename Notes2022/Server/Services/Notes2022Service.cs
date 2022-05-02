using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Grpc.Core;
using Notes2022.Proto;
using Notes2022.Shared;
using Notes2022.Server.Data;
using Notes2022.Server.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Notes2022.Server.Services
{
    public class Notes2022Service : Notes2022Server.Notes2022ServerBase
    {

        private readonly ILogger<Notes2022Service> _logger;
        private readonly NotesDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public Notes2022Service(ILogger<Notes2022Service> logger,
            NotesDbContext db,
            IConfiguration configuration,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager
          )
        {
            _logger = logger;
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public override async Task<AuthReply> Register(RegisterRequest request, ServerCallContext context)
        {
            var userExists = await _userManager.FindByEmailAsync(request.Email);
            if (userExists != null)
                return new AuthReply() { Status = StatusCodes.Status500InternalServerError, Message = "User already exists!" };

            userExists = await _userManager.FindByNameAsync(request.Username.Replace(" ", "_"));
            if (userExists != null)
                return new AuthReply() { Status = StatusCodes.Status500InternalServerError, Message = "User already exists!" };

            ApplicationUser user = new()
            {
                Email = request.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = request.Username.Replace(" ", "_"),
                DisplayName = request.Username
            };

            try
            {
                var result = await _userManager.CreateAsync(user, request.Password);
                if (!result.Succeeded)
                    return new AuthReply() { Status = StatusCodes.Status500InternalServerError, Message = "User creation failed! Please check user details and try again." };
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return new AuthReply() { Status = StatusCodes.Status500InternalServerError, Message = "User creation failed! Please check user details and try again.  " + ex.InnerException?.Message };

            }

            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));

            if (await _roleManager.RoleExistsAsync(UserRoles.User))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.User);
            }

            if (_userManager.Users.Count() == 1)
            {
                if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
                {
                    await _userManager.AddToRoleAsync(user, UserRoles.Admin);
                }
            }

            return new AuthReply() { Status = StatusCodes.Status200OK, Message = "User created!" };
        }

        public override async Task<LoginReply> Login(LoginRequest request, ServerCallContext context)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                user = await _userManager.FindByNameAsync(request.Email.Replace(" ", "_"));
            }

            if (user != null && await _signInManager.CanSignInAsync(user))
            {
                var result = await _signInManager.PasswordSignInAsync(user, request.Password, false, lockoutOnFailure: user.AccessFailedCount > _signInManager.Options.Lockout.MaxFailedAccessAttempts);

                if (!(result.Succeeded))
                {
                    await _userManager.AccessFailedAsync(user);
                    //if ( user.AccessFailedCount > _signInManager.Options.Lockout.MaxFailedAccessAttempts )
                    //{
                    //    user.LockoutEnabled = true;
                    //}
                    await _userManager.UpdateAsync(user);
                    return new LoginReply() { Status = StatusCodes.Status500InternalServerError, Message = "User Login failed! Please check user details and try again." };
                }

                var userRoles = await _userManager.GetRolesAsync(user);

                if (user.DisplayName == null)
                    user.DisplayName = String.Empty;

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email)
                };

                List<string> roles = new List<string>();
                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    roles.Add(userRole);
                }

                var token = GetToken(authClaims);

                JwtSecurityTokenHandler hand = new JwtSecurityTokenHandler();
                string stoken = hand.WriteToken(token);

                UserInfo userInfo = new UserInfo() { Displayname = user.DisplayName, Email = user.Email, Subject = user.Id,
                IsAdmin = roles.Contains("Admin"), IsUser = roles.Contains("User")};

                return new LoginReply() { Status = StatusCodes.Status200OK, Message = "Login successful.", Info = userInfo, Jwt = stoken };
            }

            return new LoginReply() { Status = StatusCodes.Status500InternalServerError, Message = "User Login failed! Please check user details and try again." };
        }

        public override async Task<AuthReply> Logout(LogoutRequest request, ServerCallContext context)
        {
            await _signInManager.SignOutAsync();
            return new AuthReply() { Status = StatusCodes.Status200OK, Message = "User logged out!" };
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTAuth:SecretKey"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWTAuth:ValidIssuerURL"],
                audience: _configuration["JWTAuth:ValidAudienceURL"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }


        [Authorize]
        public override async Task<GAppUser> GetAppUser(AppUserRequest request, ServerCallContext context)
        {
            string Id = request.Subject;
            ApplicationUser user = await _userManager.FindByIdAsync(Id);
            return user.GetGAppUser();
        }

        [Authorize]
        public override async Task<GNotefileList> GetAllNotefiles(NoRequest request, ServerCallContext context)
        {
            var x = _db.NoteFile.ToList();
            return NoteFile.GetGNotefileList(_db.NoteFile.ToList());
        }

        [Authorize(Roles = "Admin")]
        public override async Task<GNotefile?> CreateNoteFile(GNotefile request, ServerCallContext context)
        { 
            var user = context.GetHttpContext().User;
            ApplicationUser appUser = await _userManager.FindByIdAsync(user.FindFirst(ClaimTypes.NameIdentifier).Value);

            await NoteDataManager.CreateNoteFile(_db, _userManager, appUser.Id, request.NoteFileName, request.NoteFileTitle);

            List<NoteFile> x =_db.NoteFile.OrderBy(x => x.Id).ToList();
            NoteFile newfile = x[x.Count - 1];
            return newfile.GetGNotefile();
        }

        public override async Task<HomePageModel> GetHomePageModel(NoRequest request, ServerCallContext context)
        {
            return await GetBaseHomePageModelAsync(request, context);
        }

        [Authorize(Roles = "Admin")]
        public override async Task<HomePageModel> GetAdminPageModel(NoRequest request, ServerCallContext context)
        {
            HomePageModel homepageModel = await GetBaseHomePageModelAsync(request, context);

            List<ApplicationUser> udl = _db.Users.ToList();
            homepageModel.UserDataList = new GAppUserList();
            foreach (ApplicationUser userx in udl)
            {
                GAppUser ud = userx.GetGAppUser();
                homepageModel.UserDataList.List.Add(ud);
            }

            GAppUser user = homepageModel.UserData;
            homepageModel.NoteAccesses = new GNoteAccessList();
            foreach (GNotefile nf in homepageModel.NoteFiles.Notefiles)
            {
                NoteAccess na = await AccessManager.GetAccess(_db, user.Id, nf.Id, 0);
                homepageModel.NoteAccesses.List.Add(na.GetGNoteAccess());
            }

            return homepageModel;
        }

        private async Task<HomePageModel> GetBaseHomePageModelAsync(NoRequest request, ServerCallContext context)
        {
            HomePageModel homepageModel = new HomePageModel();

            NoteFile hpmf = _db.NoteFile.Where(p => p.NoteFileName == "homepagemessages").FirstOrDefault();
            if (hpmf is not null)
            {
                NoteHeader hpmh = _db.NoteHeader.Where(p => p.NoteFileId == hpmf.Id).OrderByDescending(p => p.CreateDate).FirstOrDefault();
                if (hpmh is not null)
                {
                    homepageModel.Message = _db.NoteContent.Where(p => p.NoteHeaderId == hpmh.Id).FirstOrDefault().NoteBody;
                }
            }

            if (context.GetHttpContext().User != null)
            {
                try
                {
                    ClaimsPrincipal user = context.GetHttpContext().User;
                    if (user.FindFirst(ClaimTypes.NameIdentifier) != null && user.FindFirst(ClaimTypes.NameIdentifier).Value != null)
                    {
                        ApplicationUser appUser = await _userManager.FindByIdAsync(user.FindFirst(ClaimTypes.NameIdentifier).Value);
                        homepageModel.UserData = appUser.GetGAppUser();
                        homepageModel.NoteFiles = NoteFile.GetGNotefileList(_db.NoteFile.ToList().OrderBy(p => p.NoteFileTitle).ToList());
                    }
                }
                catch (Exception ex)
                {
                }
            }

            return homepageModel;

        }

        [Authorize(Roles = "Admin")]
        public override async Task<GNotefile> UpdateNoteFile(GNotefile noteFile, ServerCallContext context)
        {
            NoteFile nf = await NoteDataManager.GetFileById(_db, noteFile.Id);
            nf.NoteFileName = noteFile.NoteFileName;
            nf.NoteFileTitle = noteFile.NoteFileTitle;
            _db.NoteFile.Update(nf);
            await _db.SaveChangesAsync();
            return noteFile;
        }

        [Authorize(Roles = "Admin")]
        public override async Task<NoRequest> DeleteNoteFile(GNotefile noteFile, ServerCallContext context)
        {
            NoteFile nf = await NoteDataManager.GetFileById(_db, noteFile.Id);
            _db.NoteFile.Remove(nf);
            await _db.SaveChangesAsync();
            return new NoRequest();
        }

        [Authorize(Roles = "Admin")]
        public override async Task<NoRequest> Import(ImportRequest request, ServerCallContext context)
        {
            Importer imp = new Importer();
            await imp.Import(_db, Globals.ImportRoot + request.UploadFile, request.NoteFile);
            return new NoRequest();
        }

        [Authorize]
        public override async Task<NoteDisplayIndexModel> GetNoteFileIndexData(NoteIndexRequest request, ServerCallContext context)
        {
            ClaimsPrincipal user;
            ApplicationUser appUser;
            NoteDisplayIndexModel idxModel = new NoteDisplayIndexModel();
            bool isAdmin;
            bool isUser;

            int arcId = 0;

            user = context.GetHttpContext().User;
            try
            {
                if (user.FindFirst(ClaimTypes.NameIdentifier) != null && user.FindFirst(ClaimTypes.NameIdentifier).Value != null)
                {
                    appUser = await _userManager.FindByIdAsync(user.FindFirst(ClaimTypes.NameIdentifier).Value);

                    isAdmin = await _userManager.IsInRoleAsync(appUser, "Admin");
                    isUser = await _userManager.IsInRoleAsync(appUser, "User");
                    if (!isUser)
                        return idxModel;    // not a User?  You get NOTHING!

                    NoteAccess noteAccess = await AccessManager.GetAccess(_db, appUser.Id, request.NoteFileId, arcId);
                    if (isAdmin)
                    {
                        noteAccess.ViewAccess = true;    // Admins can always view access list
                    }
                    idxModel.MyAccess = noteAccess.GetGNoteAccess();

                    idxModel.NoteFile = (await NoteDataManager.GetFileById(_db, request.NoteFileId)).GetGNotefile();

                    if (!idxModel.MyAccess.ReadAccess && !idxModel.MyAccess.Write)
                    {
                        idxModel.Message = "You do not have access to file " + idxModel.NoteFile.NoteFileName;
                        return idxModel;
                    }

                    List<LinkedFile> linklist = await _db.LinkedFile.Where(p => p.HomeFileId == request.NoteFileId).ToListAsync();
                    if (linklist is not null && linklist.Count > 0)
                        idxModel.LinkedText = " (Linked)";

                    List<NoteHeader> allhead = await NoteDataManager.GetAllHeaders(_db, request.NoteFileId, arcId);
                    idxModel.AllNotes = NoteHeader.GetGNoteHeaderList(allhead);

                    List<NoteHeader> notes = allhead.FindAll(p => p.ResponseOrdinal == 0).OrderBy(p => p.NoteOrdinal).ToList();
                    idxModel.Notes = NoteHeader.GetGNoteHeaderList(notes);

                    idxModel.UserData = appUser.GetGAppUser();

                    List<Tags> tags = await _db.Tags.Where(p => p.NoteFileId == request.NoteFileId && p.ArchiveId == arcId).ToListAsync();
                    idxModel.Tags = Tags.GetGTagsList(tags);

                    idxModel.ArcId = arcId;
                }
            }
            catch (Exception ex)
            {
                idxModel.Message = ex.Message;
            }

            return idxModel;
        }

        [Authorize]
        public override async Task<DisplayModel> GetNoteContent(DisplayModelRequest request, ServerCallContext context)
        {
            ClaimsPrincipal user = context.GetHttpContext().User;
            ApplicationUser appUser = await _userManager.FindByIdAsync(user.FindFirst(ClaimTypes.NameIdentifier).Value);

            bool isAdmin = await _userManager.IsInRoleAsync(appUser, "Admin");

            NoteHeader nh = await _db.NoteHeader.SingleAsync(p => p.Id == request.NoteId && p.Version == request.Vers);
            NoteContent c = await _db.NoteContent.SingleAsync(p => p.NoteHeaderId == nh.Id);
            List<Tags> tags = await _db.Tags.Where(p => p.NoteHeaderId == nh.Id).ToListAsync();
            NoteFile nf = await _db.NoteFile.SingleAsync(p => p.Id == nh.NoteFileId);
            NoteAccess access = await AccessManager.GetAccess(_db, appUser.Id, nh.NoteFileId, nh.ArchiveId);

            bool canEdit = isAdmin;         // admins can always edit a note
            if (appUser.Id == nh.AuthorID)  // otherwise only the author can edit
                canEdit = true;

            DisplayModel model = new DisplayModel()
            {
                Header = nh.GetGNoteHeader(),
                Content = c.GetGNoteContent(),
                Tags = Tags.GetGTagsList(tags),
                NoteFile = nf.GetGNotefile(),
                Access = access.GetGNoteAccess(),
                CanEdit = canEdit,
                IsAdmin = isAdmin
            };

            return model;
        }

        [Authorize]
        public override async Task<GNoteAccessList> GetAccessList(AccessAndUserListRequest request, ServerCallContext context)
        {
            return NoteAccess.GetGNoteAccessList(_db.NoteAccess.Where(p => p.NoteFileId == request.FileId && p.ArchiveId == request.ArcId).ToList());
        }

        [Authorize]
        public override async Task<AccessAndUserList> GetAccessAndUserList(AccessAndUserListRequest request, ServerCallContext context)
        {
            AccessAndUserList accessAndUserList = new AccessAndUserList();

            accessAndUserList.AccessList = NoteAccess.GetGNoteAccessList(_db.NoteAccess.Where(p => p.NoteFileId == request.FileId && p.ArchiveId == request.ArcId).ToList());
            accessAndUserList.AppUsers = ApplicationUser.GetGAppUserList((await _userManager.GetUsersInRoleAsync("User")).ToList());
            accessAndUserList.UserAccess = (await AccessManager.GetAccess(_db, request.UserId, request.FileId, request.ArcId)).GetGNoteAccess();

            return accessAndUserList;
        }

        [Authorize]
        public override async Task<GNoteAccess> UpdateAccessItem(GNoteAccess request, ServerCallContext context)
        {
            NoteAccess access = NoteAccess.GetNoteAccess(request);
            _db.NoteAccess.Update(access);
            await _db.SaveChangesAsync();

            return request;
        }

        [Authorize]
        public override async Task<NoRequest> DeleteAccessItem(GNoteAccess request, ServerCallContext context)
        {
            NoteAccess access = NoteAccess.GetNoteAccess(request);
            _db.NoteAccess.Remove(access);
            await _db.SaveChangesAsync();

            return new NoRequest();
        }

        [Authorize]
        public override async Task<GNoteAccess> AddAccessItem(GNoteAccess request, ServerCallContext context)
        {
            await _db.NoteAccess.AddAsync(NoteAccess.GetNoteAccess(request));
            await _db.SaveChangesAsync();

            return request;
        }

        [Authorize]
        public override async Task<GAppUser> GetUserData(NoRequest request, ServerCallContext context)
        {
            ClaimsPrincipal user = context.GetHttpContext().User;
            ApplicationUser appUser = await _userManager.FindByIdAsync(user.FindFirst(ClaimTypes.NameIdentifier).Value);
            return appUser.GetGAppUser();
        }


        [Authorize]
        public override async Task<GAppUser> UpdateUserData(GAppUser request, ServerCallContext context)
        {
            ApplicationUser appUserBase = await _userManager.FindByIdAsync(request.Id);
            ApplicationUser merged = ApplicationUser.MergeApplicationUser(request, appUserBase);

            await _userManager.UpdateAsync(merged);

            return request;
        }

        [Authorize]
        public override async Task<GNoteHeaderList> GetVersions(GetVersionsRequest request, ServerCallContext context)
        {
            List<NoteHeader> hl;

            hl = _db.NoteHeader.Where(p => p.NoteFileId == request.FileId && p.Version != 0
                    && p.NoteOrdinal == request.NoteOrdinal && p.ResponseOrdinal == request.ResponseOrdinal && p.ArchiveId == request.ArcId)
                .OrderBy(p => p.Version)
                .ToList();

            return NoteHeader.GetGNoteHeaderList(hl);
        }

        [Authorize]
        public override async Task<GSequencerList> GetSequencer(NoRequest request, ServerCallContext context)
        {
            ClaimsPrincipal user = context.GetHttpContext().User;
            ApplicationUser appUser = await _userManager.FindByIdAsync(user.FindFirst(ClaimTypes.NameIdentifier).Value);

            // My list
            List<Sequencer> mine = await _db.Sequencer.Where(p => p.UserId == appUser.Id).OrderBy(p => p.Ordinal).ThenBy(p => p.LastTime).ToListAsync();

            if (mine is null)
                mine = new List<Sequencer>();

            List<Sequencer> avail = new List<Sequencer>();

            foreach (Sequencer m in mine)
            {
                NoteAccess na = await AccessManager.GetAccess(_db, appUser.Id, m.NoteFileId, 0);
                if (na.ReadAccess)
                    avail.Add(m);   // ONLY if you have current read access!!
            }
            return Sequencer.GetGSequencerList( avail.OrderBy(p => p.Ordinal).ToList());
        }

        [Authorize]
        public override async Task<NoRequest> CreateSequencer(SCheckModel request, ServerCallContext context)
        {
            ClaimsPrincipal user = context.GetHttpContext().User;
            ApplicationUser appUser = await _userManager.FindByIdAsync(user.FindFirst(ClaimTypes.NameIdentifier).Value);

            List<Sequencer> mine = await _db.Sequencer.Where(p => p.UserId == appUser.Id).OrderByDescending(p => p.Ordinal).ToListAsync();

            int ord;
            if (mine is null || mine.Count == 0)
            {
                ord = 1;
            }
            else
            {
                ord = mine[0].Ordinal + 1;
            }


            Sequencer tracker = new Sequencer   // make a starting entry
            {
                Active = true,
                NoteFileId = request.FileId,
                LastTime = DateTime.UtcNow,
                UserId = appUser.Id,
                Ordinal = ord,
                StartTime = DateTime.UtcNow
            };

            _db.Sequencer.Add(tracker);
            await _db.SaveChangesAsync();

            return new NoRequest();
        }

        [Authorize]
        public override async Task<NoRequest> DeleteSequencer(SCheckModel request, ServerCallContext context)
        {
            ClaimsPrincipal user = context.GetHttpContext().User;
            ApplicationUser appUser = await _userManager.FindByIdAsync(user.FindFirst(ClaimTypes.NameIdentifier).Value);

            Sequencer mine = await _db.Sequencer.SingleOrDefaultAsync(p => p.UserId == appUser.Id && p.NoteFileId == request.FileId);
            if (mine is null)
                return new NoRequest();

            _db.Sequencer.Remove(mine);
            await _db.SaveChangesAsync();

            return new NoRequest();
        }

        [Authorize]
        public override async Task<NoRequest> UpdateSequencerOrdinal(GSequencer request, ServerCallContext context)
        {
            Sequencer modified = await _db.Sequencer.SingleAsync(p => p.UserId == request.UserId && p.NoteFileId == request.NoteFileId);

            modified.LastTime = request.LastTime.ToDateTime();
            modified.Ordinal = request.Ordinal;

            _db.Entry(modified).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            return new NoRequest();
        }

        [Authorize]
        public override async Task<NoRequest> UpdateSequencer(GSequencer request, ServerCallContext context)
        {
            Sequencer modified = await _db.Sequencer.SingleAsync(p => p.UserId == request.UserId && p.NoteFileId == request.NoteFileId);

            modified.Active = request.Active;
            if (request.Active)  // starting to seq - set start time
            {
                modified.StartTime = DateTime.UtcNow;
            }
            else            // end of a file - copy start time to LastTime so we do not miss notes
            {
                modified.LastTime = modified.StartTime;  //request.StartTime.ToDateTime();
            }

            _db.Entry(modified).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            return new NoRequest();
        }

        [Authorize]
        public override async Task<GNotefile> GetNoteFile(NoteIndexRequest request, ServerCallContext context)
        {
            NoteFile nf = _db.NoteFile.Single(p => p.Id == request.NoteFileId);

            return nf.GetGNotefile();
        }

        [Authorize]
        public override async Task<GNoteHeader> CreateNewNote(TextViewModel tvm, ServerCallContext context)
        {
            if (tvm.MyNote is null || tvm.MySubject is null)
                return new GNoteHeader();

            ClaimsPrincipal user = context.GetHttpContext().User;
            ApplicationUser appUser = await _userManager.FindByIdAsync(user.FindFirst(ClaimTypes.NameIdentifier).Value);
            bool test = await _userManager.IsInRoleAsync(appUser, "User");
            if (!test)  // Must be in a User Role
                return new GNoteHeader();

            ApplicationUser me = appUser;
            DateTime now = DateTime.UtcNow;
            NoteHeader nheader = new()  // construct a new NoteHeader
            {
                LastEdited = now,
                ThreadLastEdited = now,
                CreateDate = now,
                NoteFileId = tvm.NoteFileID,
                AuthorName = me.DisplayName,
                AuthorID = me.Id,
                NoteSubject = tvm.MySubject,
                DirectorMessage = tvm.DirectorMessage,
                ResponseOrdinal = 0,
                ResponseCount = 0
            };

            NoteHeader created;

            if (tvm.BaseNoteHeaderID == 0)  // a base note
            {
                created = await NoteDataManager.CreateNote(_db, nheader, tvm.MyNote, tvm.TagLine, tvm.DirectorMessage, true, false);
            }
            else        // a response
            {
                nheader.BaseNoteId = tvm.BaseNoteHeaderID;
                nheader.RefId = tvm.RefId;
                created = await NoteDataManager.CreateResponse(_db, nheader, tvm.MyNote, tvm.TagLine, tvm.DirectorMessage, true, false);
            }

            //// Process any linked note file
            //await ProcessLinkedNotes();

            //// Send copy to subscribers
            //await SendNewNoteToSubscribers(created);

            return created.GetGNoteHeader();
        }

        //[Authorize]
        //public override async Task<GNoteHeader> GetNewestNote(NoRequest request, ServerCallContext context)
        //{
        //    NoteHeader nh = _db.NoteHeader.OrderByDescending(p => p.Id).FirstOrDefault();
        //    return nh.GetGNoteHeader();
        //}

        [Authorize]
        public override async Task<GNoteHeader> UpdateNote(TextViewModel tvm, ServerCallContext context)
        {
            if (tvm.MyNote is null || tvm.MySubject is null)
                return new GNoteHeader();

            NoteHeader nheader = await NoteDataManager.GetBaseNoteHeaderById(_db, tvm.NoteID);

            // update header
            DateTime now = DateTime.UtcNow;
            nheader.NoteSubject = tvm.MySubject;
            nheader.DirectorMessage = tvm.DirectorMessage;
            //nheader.LastEdited = now;
            nheader.ThreadLastEdited = now;

            NoteContent nc = new()
            {
                NoteHeaderId = tvm.NoteID,
                NoteBody = tvm.MyNote
            };

            NoteHeader newheader  = await NoteDataManager.EditNote(_db, _userManager, nheader, nc, tvm.TagLine);

            //await ProcessLinkedNotes();

            return newheader.GetGNoteHeader();
        }

        [Authorize]
        public override async Task<GNoteHeader> GetHeaderForNoteId(NoteId request, ServerCallContext context)
        {
            return (await _db.NoteHeader.SingleAsync(p => p.Id == request.Id)).GetGNoteHeader();
        }

        public override async Task<AboutModel> GetAbout(NoRequest request, ServerCallContext context)
        {
            return new AboutModel()
            {
                PrimeAdminEmail = _configuration["PrimeAdminEmail"],
                PrimeAdminName = _configuration["PrimeAdminName"]
            };
        }
    }
}
