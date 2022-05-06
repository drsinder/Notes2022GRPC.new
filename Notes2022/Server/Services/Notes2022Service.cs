/*--------------------------------------------------------------------------
    **
    ** Copyright © 2022, Dale Sinder
    **
    ** Name: Notes2022Service.cs
    **
    ** Description:
    **      Service implementation for gRPC protocol for app
    **
    ** This program is free software: you can redistribute it and/or modify
    ** it under the terms of the GNU General Public License version 3 as
    ** published by the Free Software Foundation.   
    **
    ** This program is distributed in the hope that it will be useful,
    ** but WITHOUT ANY WARRANTY; without even the implied warranty of
    ** MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
    ** GNU General Public License version 3 for more details.
    **
    **  You should have received a copy of the GNU General Public License
    **  version 3 along with this program in file "license-gpl-3.0.txt".
    **  If not, see<http://www.gnu.org/licenses/gpl-3.0.txt>.
    **
    **--------------------------------------------------------------------------*/


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
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Text.Json;

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
        private readonly IEmailSender _emailSender;

        public Notes2022Service(ILogger<Notes2022Service> logger,
            NotesDbContext db,
            IConfiguration configuration,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            UserManager<ApplicationUser> userManager
          )
        {
            _logger = logger;
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _emailSender = emailSender;
        }

        public override async Task<NoRequest> SpinUp(NoRequest request, ServerCallContext context)
        {
            return new NoRequest();
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
                DisplayName = request.Username,
                Ipref2 = 12     // starting note index page size pref.              
            };

            try
            {
                var result = await _userManager.CreateAsync(user, request.Password);
                if (!result.Succeeded)
                    return new AuthReply() { Status = StatusCodes.Status500InternalServerError, Message = "User creation failed! Please check user details and try again." };
            }
            catch (Exception ex)
            {
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

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            ConfirmEmailRequest mess = new ConfirmEmailRequest() { UserId = user.Id, Code = code };
            string payload = Globals.Base64Encode(JsonSerializer.Serialize(mess));

            string target = _configuration["AppUrl"] + "/authentication/confirmemail/" + payload;
            await _emailSender.SendEmailAsync(request.Email, "Confirm your email",
                $"Please confirm your Notes 2022 account email by <a href='{target}'>clicking here</a>.");

            return new AuthReply() { Status = StatusCodes.Status200OK, Message = "User created!" };
        }

        public override async Task<AuthReply> ConfirmEmail(ConfirmEmailRequest request, ServerCallContext context)
        {
            AuthReply ret = new AuthReply();
            ret.Status = StatusCodes.Status500InternalServerError;

            if (request.Code is null || request.UserId is null)
            {
                ret.Message = "Improper request! (null elements)";
                return ret;
            }

            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user is null)
            {
                ret.Message = "Failed to find user!";
                return ret;
            }

            var result = await _userManager.ConfirmEmailAsync(user, request.Code);
            if ( result.Succeeded)
            {
                ret.Status = StatusCodes.Status200OK;
                ret.Message = "Thank you for confirming your email " + user.DisplayName + ".  You may now log in!";
                return ret;
            }

            ret.Message = "Failed to confirm email address.";

            return ret;
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

                var token = GetToken(authClaims, request.Hours);

                JwtSecurityTokenHandler hand = new JwtSecurityTokenHandler();
                string stoken = hand.WriteToken(token);

                UserInfo userInfo = new UserInfo() { Displayname = user.DisplayName, Email = user.Email, Subject = user.Id,
                IsAdmin = roles.Contains("Admin"), IsUser = roles.Contains("User")};

                return new LoginReply() { Status = StatusCodes.Status200OK, Message = "Login successful.", Info = userInfo, Jwt = stoken };
            }

            return new LoginReply() { Status = StatusCodes.Status500InternalServerError, Message = "User Login failed! Please check user details and try again." };
        }

        public override async Task<AuthReply> Logout(NoRequest request, ServerCallContext context)
        {
            await _signInManager.SignOutAsync();
            return new AuthReply() { Status = StatusCodes.Status200OK, Message = "User logged out!" };
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims, int hours)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTAuth:SecretKey"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWTAuth:ValidIssuerURL"],
                audience: _configuration["JWTAuth:ValidAudienceURL"],
                expires: DateTime.Now.AddHours(hours),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }


        //[Authorize]
        //public override async Task<GAppUser> GetAppUser(AppUserRequest request, ServerCallContext context)
        //{
        //    string Id = request.Subject;
        //    ApplicationUser user = await _userManager.FindByIdAsync(Id);
        //    return user.GetGAppUser();
        //}

        [Authorize (Roles = "Admin")]
        public override async Task<GAppUserList> GetUserList(NoRequest request, ServerCallContext context)
        {
            List<ApplicationUser> list = _userManager.Users.ToList();
            return Notes2022.Server.Entities.ApplicationUser.GetGAppUserList(list);
        }

        [Authorize(Roles = "Admin")]
        public override async Task<EditUserViewModel> GetUserRoles(AppUserRequest request, ServerCallContext context)
        {
            EditUserViewModel model = new EditUserViewModel();
            model.RolesList = new CheckedUserList();
            string Id = request.Subject;
            ApplicationUser user = await _userManager.FindByIdAsync(Id);

            model.UserData = user.GetGAppUser();

            var allRoles = _roleManager.Roles.ToList();

            //var myRoles = await _userManager.GetRolesAsync(user);

            foreach (var role in allRoles)
            {
                CheckedUser cu = new CheckedUser();
                cu.TheRole = new UserRole();
                cu.TheRole.RoleId = role.Id;
                cu.TheRole.RoleName = role.Name;
                cu.IsMember = await _userManager.IsInRoleAsync(user, role.Name);
                model.RolesList.List.Add(cu);
            }


            return model;
        }

        [Authorize (Roles="Admin")]
        public override async Task<NoRequest> UpdateUserRoles(EditUserViewModel model, ServerCallContext context)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(model.UserData.Id);
            var myRoles = await _userManager.GetRolesAsync(user);
            foreach (CheckedUser item in model.RolesList.List)
            {
                if (item.IsMember && !myRoles.Contains(item.TheRole.RoleName)) // need to add role
                {
                    await _userManager.AddToRoleAsync(user, item.TheRole.RoleName);
                }
                else if (!item.IsMember && myRoles.Contains(item.TheRole.RoleName)) // need to remove role
                {
                    await _userManager.RemoveFromRoleAsync(user, item.TheRole.RoleName);
                }
            }


            return new NoRequest();
        }

        //[Authorize]
        //public override async Task<GNotefileList> GetAllNotefiles(NoRequest request, ServerCallContext context)
        //{
        //    List<NoteFile> x = _db.NoteFile.ToList();

        //    // should filter out files user has no access to  TODO

        //    return NoteFile.GetGNotefileList(x);
        //}

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

        [Authorize]
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

                        List<NoteFile> allFiles = _db.NoteFile.ToList().OrderBy(p => p.NoteFileTitle).ToList();
                        List<NoteAccess> myAccesses = _db.NoteAccess.Where(p => p.UserID == appUser.Id).ToList();
                        List<NoteAccess> otherAccesses = _db.NoteAccess.Where(p => p.UserID == Globals.AccessOtherId).ToList();

                        List<NoteFile> myNoteFiles = new List<NoteFile>();

                        bool isAdmin = await _userManager.IsInRoleAsync(appUser, UserRoles.Admin);
                        foreach (NoteFile file in allFiles)
                        {
                            NoteAccess? x = myAccesses.SingleOrDefault(p => p.NoteFileId == file.Id);
                            if (x is null)
                                x = otherAccesses.Single(p => p.NoteFileId == file.Id);

                            if (isAdmin || x.ReadAccess || x.Write || x.ViewAccess)
                            {
                                myNoteFiles.Add(file);
                            }
                        }

                        homepageModel.NoteFiles = NoteFile.GetGNotefileList(myNoteFiles);
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

            // remove tags
            List<Tags> tl = _db.Tags.Where(p => p.NoteFileId == nf.Id).ToList();
            _db.Tags.RemoveRange(tl);

            // remove content
            List <NoteHeader> hl = _db.NoteHeader.Where(p => p.NoteFileId == nf.Id).ToList();
            foreach (NoteHeader h in hl)
            {
                NoteContent c = _db.NoteContent.Single(p => p.NoteHeaderId == h.Id);
                _db.NoteContent.Remove(c);
            }

            // remove headers
            _db.NoteHeader.RemoveRange(hl);

            // remove access
            List<NoteAccess> al = _db.NoteAccess.Where(p => p.NoteFileId == nf.Id).ToList();
            _db.NoteAccess.RemoveRange(al);

            // remove file
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

            NoteAccess na = await AccessManager.GetAccess(_db, appUser.Id, nh.NoteFileId, nh.ArchiveId);

            if (!na.ReadAccess)
                return new DisplayModel();

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
            ClaimsPrincipal user = context.GetHttpContext().User;
            ApplicationUser appUser = await _userManager.FindByIdAsync(user.FindFirst(ClaimTypes.NameIdentifier).Value);
            NoteAccess na = await AccessManager.GetAccess(_db, appUser.Id, access.NoteFileId, access.ArchiveId);
            if (na.EditAccess)
            {
                _db.NoteAccess.Update(access);
                await _db.SaveChangesAsync();
            }

            return request;
        }

        [Authorize]
        public override async Task<NoRequest> DeleteAccessItem(GNoteAccess request, ServerCallContext context)
        {
            NoteAccess access = NoteAccess.GetNoteAccess(request);
            ClaimsPrincipal user = context.GetHttpContext().User;
            ApplicationUser appUser = await _userManager.FindByIdAsync(user.FindFirst(ClaimTypes.NameIdentifier).Value);
            NoteAccess na = await AccessManager.GetAccess(_db, appUser.Id, access.NoteFileId, access.ArchiveId);
            if (na.EditAccess)
            {
                _db.NoteAccess.Remove(access);
                await _db.SaveChangesAsync();
            }

            return new NoRequest();
        }

        [Authorize]
        public override async Task<GNoteAccess> AddAccessItem(GNoteAccess request, ServerCallContext context)
        {
            ClaimsPrincipal user = context.GetHttpContext().User;
            ApplicationUser appUser = await _userManager.FindByIdAsync(user.FindFirst(ClaimTypes.NameIdentifier).Value);
            NoteAccess na = await AccessManager.GetAccess(_db, appUser.Id, request.NoteFileId, request.ArchiveId);
            if (na.EditAccess)
            {
                await _db.NoteAccess.AddAsync(NoteAccess.GetNoteAccess(request));
                await _db.SaveChangesAsync();
            }

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
            ClaimsPrincipal user = context.GetHttpContext().User;
            ApplicationUser appUser = await _userManager.FindByIdAsync(user.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (appUser.Id != request.Id)   // can onlt update self
                return request;

            ApplicationUser appUserBase = await _userManager.FindByIdAsync(request.Id);
            ApplicationUser merged = ApplicationUser.MergeApplicationUser(request, appUserBase);

            await _userManager.UpdateAsync(merged);

            return request;
        }

        [Authorize]
        public override async Task<GNoteHeaderList> GetVersions(GetVersionsRequest request, ServerCallContext context)
        {
            ClaimsPrincipal user = context.GetHttpContext().User;
            ApplicationUser appUser = await _userManager.FindByIdAsync(user.FindFirst(ClaimTypes.NameIdentifier).Value);

            NoteAccess na = await AccessManager.GetAccess(_db, appUser.Id, request.FileId, request.ArcId);
            if (!na.ReadAccess)
                return new GNoteHeaderList();

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
            ClaimsPrincipal user = context.GetHttpContext().User;
            ApplicationUser appUser = await _userManager.FindByIdAsync(user.FindFirst(ClaimTypes.NameIdentifier).Value);
            NoteAccess na = await AccessManager.GetAccess(_db, appUser.Id, request.NoteFileId, 0);
            if (na.Write )
                ;
            else
                return new GNotefile();

            NoteFile nf = _db.NoteFile.Single(p => p.Id == request.NoteFileId);

            return nf.GetGNotefile();
        }

        public override async Task<GNoteHeader> CreateNewNote(TextViewModel tvm, ServerCallContext context)
        {
            if (tvm.MyNote is null || tvm.MySubject is null)
                return new GNoteHeader();

            ClaimsPrincipal user = context.GetHttpContext().User;
            ApplicationUser appUser = await _userManager.FindByIdAsync(user.FindFirst(ClaimTypes.NameIdentifier).Value);
            bool test = await _userManager.IsInRoleAsync(appUser, "User");
            if (!test)  // Must be in a User Role
                return new GNoteHeader();

            NoteAccess na = await AccessManager.GetAccess(_db, appUser.Id, tvm.NoteFileID, 0);
            if (na.Write || na.Respond)
                ;
            else
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

            ClaimsPrincipal user = context.GetHttpContext().User;
            ApplicationUser appUser = await _userManager.FindByIdAsync(user.FindFirst(ClaimTypes.NameIdentifier).Value);
            bool isAdmin = await _userManager.IsInRoleAsync(appUser, "Admin");
            bool canEdit = isAdmin;         // admins can always edit a note
            if (appUser.Id == nheader.AuthorID)  // otherwise only the author can edit
                canEdit = true;

            if (!canEdit)
                return new GNoteHeader();

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
            ClaimsPrincipal user = context.GetHttpContext().User;
            ApplicationUser appUser = await _userManager.FindByIdAsync(user.FindFirst(ClaimTypes.NameIdentifier).Value);
            bool isAdmin = await _userManager.IsInRoleAsync(appUser, "Admin");
            
            GNoteHeader gnh = (await _db.NoteHeader.SingleAsync(p => p.Id == request.Id)).GetGNoteHeader();

            if (isAdmin)
                ;
            else
            {
                NoteAccess na = await AccessManager.GetAccess(_db, appUser.Id, gnh.NoteFileId, gnh.ArchiveId);
                if (!na.ReadAccess)
                {
                    return new GNoteHeader();
                }
            }

            return gnh;
        }

        public override async Task<AboutModel> GetAbout(NoRequest request, ServerCallContext context)
        {
            return new AboutModel()
            {
                PrimeAdminEmail = _configuration["PrimeAdminEmail"],
                PrimeAdminName = _configuration["PrimeAdminName"]
            };
        }

        private static int throttle = 0;
        private static DateTime? TimeOfThrottle = null;

        public override async Task<NoRequest> SendEmail(GEmail request, ServerCallContext context)
        {
            try
            {
                ClaimsPrincipal user = context.GetHttpContext().User;
                ApplicationUser appUser = await _userManager.FindByIdAsync(user.FindFirst(ClaimTypes.NameIdentifier).Value);
            }
            catch (Exception ex)
            {
                // was not authenticated - slow them up

                if (throttle++ >= 100)
                { 
                    // some real potential abuse??
                    Thread.Sleep(1000 * throttle);

                    if (TimeOfThrottle is null)
                    {
                        TimeOfThrottle = DateTime.UtcNow;
                    }
                    else
                    {
                        TimeSpan? diff = DateTime.UtcNow - TimeOfThrottle;
                        if (diff > new TimeSpan(0, 30, 0)) // backoff in 30 minutes
                        {
                            throttle = 0;
                            TimeOfThrottle = null;
                        }
                    }

                }

                Thread.Sleep(1000);
            }

            await _emailSender.SendEmailAsync(request.Address, request.Subject, request.Body);
            return new NoRequest();
        }

        [Authorize]
        public override async Task<NoRequest> SendEmailAuth(GEmail request, ServerCallContext context)
        {
            await _emailSender.SendEmailAsync(request.Address, request.Subject, request.Body);
            return new NoRequest();
        }

        [Authorize]
        public override async Task<GNoteHeaderList> GetExport(ExportRequest request, ServerCallContext context)
        {
            ClaimsPrincipal user = context.GetHttpContext().User;
            ApplicationUser appUser = await _userManager.FindByIdAsync(user.FindFirst(ClaimTypes.NameIdentifier).Value);
            NoteAccess na = await AccessManager.GetAccess(_db, appUser.Id, request.FileId, request.ArcId);
            if (!na.ReadAccess)
                return new GNoteHeaderList();

            List<NoteHeader> nhl;

            if (request.NoteOrdinal == 0)   // All base notes
            {
                nhl = await _db.NoteHeader
                    .Where(p => p.NoteFileId == request.FileId && p.ArchiveId == request.ArcId && p.ResponseOrdinal == 0)
                    .OrderBy(p => p.NoteOrdinal)
                    .ToListAsync();
            }
            else                // Just one base note/response
            {
                nhl = await _db.NoteHeader
                    .Where(p => p.NoteFileId == request.FileId && p.ArchiveId == request.ArcId && p.NoteOrdinal == request.NoteOrdinal && p.ResponseOrdinal == request.ResponseOrdinal)
                    .ToListAsync();
            }

            return NoteHeader.GetGNoteHeaderList(nhl);
        }

        [Authorize]
        public override async Task<GNoteContent> GetExport2(NoteId request, ServerCallContext context)
        {
            NoteContent? nc = await _db.NoteContent
                .Where(p => p.NoteHeaderId == request.Id)
                .FirstOrDefaultAsync();

            NoteHeader? nh = await _db.NoteHeader
                .Where(p => p.Id == request.Id)
                .FirstOrDefaultAsync();

            ClaimsPrincipal user = context.GetHttpContext().User;
            ApplicationUser appUser = await _userManager.FindByIdAsync(user.FindFirst(ClaimTypes.NameIdentifier).Value);
            NoteAccess na = await AccessManager.GetAccess(_db, appUser.Id, nh.NoteFileId, nh.ArchiveId);
            if (!na.ReadAccess)
                return new GNoteContent();


            return nc.GetGNoteContent();
        }

        [Authorize]
        public override async Task<NoRequest> DoForward(ForwardViewModel fv, ServerCallContext context)
        {
            ClaimsPrincipal user = context.GetHttpContext().User;
            ApplicationUser appUser = await _userManager.FindByIdAsync(user.FindFirst(ClaimTypes.NameIdentifier).Value);
            NoteAccess na = await AccessManager.GetAccess(_db, appUser.Id, fv.FileID, fv.ArcID);
            if (!na.ReadAccess)
                return new NoRequest();

            string myEmail = await LocalService.MakeNoteForEmail(fv, fv.NoteFile, _db, appUser.Email, appUser.DisplayName);

            await _emailSender.SendEmailAsync(appUser.Email, fv.NoteSubject, myEmail);
            return new NoRequest();
        }

        [Authorize]
        public override async Task<GNotefileList> GetNoteFilesOrderedByName(NoRequest request, ServerCallContext context)
        {
            List<NoteFile> noteFiles = _db.NoteFile.OrderBy(p => p.NoteFileName).ToList();
            return NoteFile.GetGNotefileList(noteFiles);
        }

        [Authorize]
        public override async Task<NoRequest> CopyNote(CopyModel Model, ServerCallContext context)
        {
            ClaimsPrincipal user = context.GetHttpContext().User;
            ApplicationUser appUser = await _userManager.FindByIdAsync(user.FindFirst(ClaimTypes.NameIdentifier).Value);

            NoteAccess na = await AccessManager.GetAccess(_db, appUser.Id, Model.Note.NoteFileId, Model.Note.ArchiveId);
            if (!na.ReadAccess)
                return new NoRequest();         // can not read file

            int fileId = Model.FileId;

            // Can I write to the target file?
            string uid = appUser.Id;
            NoteAccess myAccess = await AccessManager.GetAccess(_db, uid, fileId, 0);
            if (!myAccess.Write)
                return new NoRequest();         // can not write to file

            // Prepare to copy
            NoteHeader Header = NoteHeader.GetNoteHeader(Model.Note);
            bool whole = Model.WholeString;
            NoteFile noteFile = await _db.NoteFile.SingleAsync(p => p.Id == fileId);

            // Just the note
            if (!whole)
            {
                NoteContent cont = await _db.NoteContent.SingleAsync(p => p.NoteHeaderId == Header.Id);
                //cont.NoteHeader = null;
                List<Tags> tags = await _db.Tags.Where(p => p.NoteHeaderId == Header.Id).ToListAsync();

                string Body = string.Empty;
                Body = MakeHeader(Header, noteFile);
                Body += cont.NoteBody;

                Header = Header.CloneForLink();

                Header.Id = 0;
                Header.ArchiveId = 0;
                Header.LinkGuid = string.Empty;
                Header.NoteOrdinal = 0;
                Header.ResponseCount = 0;
                Header.NoteFileId = fileId;
                Header.BaseNoteId = 0;
                //Header.NoteFile = null;
                Header.AuthorID = appUser.Id;
                Header.AuthorName = appUser.DisplayName;

                Header.CreateDate = Header.ThreadLastEdited = Header.LastEdited = DateTime.Now.ToUniversalTime();

                await NoteDataManager.CreateNote(_db, Header, Body, Tags.ListToString(tags), Header.DirectorMessage, true, false);

                new NoRequest();
            }
            else    // whole note string
            {
                // get base note first
                NoteHeader BaseHeader;
                BaseHeader = await _db.NoteHeader.SingleAsync(p => p.NoteFileId == Header.NoteFileId
                    && p.ArchiveId == Header.ArchiveId
                    && p.NoteOrdinal == Header.NoteOrdinal
                    && p.ResponseOrdinal == 0);

                Header = BaseHeader.CloneForLink();

                NoteContent cont = await _db.NoteContent.SingleAsync(p => p.NoteHeaderId == Header.Id);
                //cont.NoteHeader = null;
                List<Tags> tags = await _db.Tags.Where(p => p.NoteHeaderId == Header.Id).ToListAsync();

                string Body = string.Empty;
                Body = MakeHeader(Header, noteFile);
                Body += cont.NoteBody;

                Header.Id = 0;
                Header.ArchiveId = 0;
                Header.LinkGuid = string.Empty;
                Header.NoteOrdinal = 0;
                Header.ResponseCount = 0;
                Header.NoteFileId = fileId;
                Header.BaseNoteId = 0;
                //Header.NoteFile = null;
                Header.AuthorID = appUser.Id;
                Header.AuthorName = appUser.DisplayName;

                Header.CreateDate = Header.ThreadLastEdited = Header.LastEdited = DateTime.Now.ToUniversalTime();

                Header.NoteContent = null;

                NoteHeader NewHeader = await NoteDataManager.CreateNote(_db, Header, Body, Tags.ListToString(tags), Header.DirectorMessage, true, false);

                // now deal with any responses
                for (int i = 1; i <= BaseHeader.ResponseCount; i++)
                {
                    NoteHeader RHeader = await _db.NoteHeader.SingleAsync(p => p.NoteFileId == BaseHeader.NoteFileId
                        && p.ArchiveId == BaseHeader.ArchiveId
                        && p.NoteOrdinal == BaseHeader.NoteOrdinal
                        && p.ResponseOrdinal == i);

                    Header = RHeader.CloneForLinkR();

                    cont = await _db.NoteContent.SingleAsync(p => p.NoteHeaderId == Header.Id);
                    tags = await _db.Tags.Where(p => p.NoteHeaderId == Header.Id).ToListAsync();

                    Body = string.Empty;
                    Body = MakeHeader(Header, noteFile);
                    Body += cont.NoteBody;

                    Header.Id = 0;
                    Header.ArchiveId = 0;
                    Header.LinkGuid = string.Empty;
                    Header.NoteOrdinal = NewHeader.NoteOrdinal;
                    Header.ResponseCount = 0;
                    Header.NoteFileId = fileId;
                    Header.BaseNoteId = NewHeader.Id;
                    //Header.NoteFile = null;
                    Header.ResponseOrdinal = 0;
                    Header.AuthorID = appUser.Id;
                    Header.AuthorName = appUser.DisplayName;

                    Header.CreateDate = Header.ThreadLastEdited = Header.LastEdited = DateTime.Now.ToUniversalTime();

                    await NoteDataManager.CreateResponse(_db, Header, Body, Tags.ListToString(tags), Header.DirectorMessage, true, false);
                }

            }
                return new NoRequest();
        }

        // Utility method - makes a viewable header for the copied note
        private string MakeHeader(NoteHeader header, NoteFile noteFile)
        {
            StringBuilder sb = new();

            sb.Append("<div class=\"copiednote\">From: ");
            sb.Append(noteFile.NoteFileName);
            sb.Append(" - ");
            sb.Append(header.NoteSubject);
            sb.Append(" - ");
            sb.Append(header.AuthorName);
            sb.Append(" - ");
            sb.Append(header.CreateDate.ToShortDateString());
            sb.AppendLine("</div>");
            return sb.ToString();
        }

        [Authorize]
        public override async Task<NoRequest> DeleteNote(NoteId request, ServerCallContext context)
        {
            NoteHeader note = await NoteDataManager.GetNoteByIdWithFile(_db, request.Id);

            ClaimsPrincipal user = context.GetHttpContext().User;
            ApplicationUser appUser = await _userManager.FindByIdAsync(user.FindFirst(ClaimTypes.NameIdentifier).Value);
            NoteAccess na = await AccessManager.GetAccess(_db, appUser.Id, note.NoteFileId, note.ArchiveId);
            if (!na.DeleteEdit)
            {
                new NoRequest();
            }

            await NoteDataManager.DeleteNote(_db, note);

            return new NoRequest();
        }

        [Authorize]
        public override async Task<JsonExport> GetExportJson(ExportRequest request, ServerCallContext context)
        {
            JsonExport stuff = new JsonExport();

            stuff.NoteFile = _db.NoteFile.Single(p => p.Id == request.FileId).GetGNotefile();

            ClaimsPrincipal user = context.GetHttpContext().User;
            ApplicationUser appUser = await _userManager.FindByIdAsync(user.FindFirst(ClaimTypes.NameIdentifier).Value);
            NoteAccess na = await AccessManager.GetAccess(_db, appUser.Id, stuff.NoteFile.Id, 0);
            if (!na.ReadAccess)
                return new JsonExport();


            stuff.NoteHeaders = NoteHeader.GetGNoteHeaderList(
                await _db.NoteHeader
                    .Where(p => p.NoteFileId == request.FileId && p.ArchiveId == request.ArcId && !p.IsDeleted)
                    .OrderBy(p => p.NoteOrdinal)
                    .ThenBy(p => p.ResponseOrdinal)
                    .ToListAsync());

            foreach (GNoteHeader item in stuff.NoteHeaders.List)
            {
                item.Content = (await _db.NoteContent
                    .SingleAsync(p => p.NoteHeaderId == item.Id)).GetGNoteContent();

                var x = await _db.Tags.Where(p => p.NoteHeaderId == item.Id).ToListAsync();
                item.Tags = Tags.GetGTagsList(x);
            }

            return stuff;
        }
    }
}
