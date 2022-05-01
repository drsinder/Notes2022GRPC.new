using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Notes2022.Proto;

namespace Notes2022.Server.Entities
{
    /// <summary>
    /// Extentions to the base IdentityUser
    /// 
    /// Contains fields mirrored locally in UserData
    /// 
    /// These fields are accessed and edited there and then written back
    /// enmass.  By contrast the predefined field not seen here are
    /// almost always accessed via methods.  These methods create a Validation 
    /// Stamp for the predefined fields.  Tinker with those directly and
    /// you will probably make the user "Unusable".
    /// 
    /// </summary>
    public class ApplicationUser : IdentityUser
    {

        //[Required]
        [Display(Name = "Display Name")]
        [StringLength(50)]
        [PersonalData]
        public string? DisplayName { get; set; }

        [PersonalData]
        public int TimeZoneID { get; set; }

        [PersonalData]
        public int Ipref0 { get; set; }

        [PersonalData]
        public int Ipref1 { get; set; }

        [PersonalData]
        public int Ipref2 { get; set; } // user choosen page size

        [PersonalData]
        public int Ipref3 { get; set; }

        [PersonalData]
        public int Ipref4 { get; set; }

        [PersonalData]
        public int Ipref5 { get; set; }

        [PersonalData]
        public int Ipref6 { get; set; }

        [PersonalData]
        public int Ipref7 { get; set; }

        [PersonalData]
        public int Ipref8 { get; set; }

        [PersonalData]
        public int Ipref9 { get; set; } // bits extend bool properties


        [PersonalData]
        public bool Pref0 { get; set; }

        [PersonalData]
        public bool Pref1 { get; set; } // false = use paged note index, true= scrolled

        [PersonalData]
        public bool Pref2 { get; set; } // use alternate editor

        [PersonalData]
        public bool Pref3 { get; set; } // show responses by default

        [PersonalData]
        public bool Pref4 { get; set; } // multiple expanded responses

        [PersonalData]
        public bool Pref5 { get; set; } // expanded responses

        [PersonalData]
        public bool Pref6 { get; set; } // alternate text editor

        [PersonalData]
        public bool Pref7 { get; set; } // show content when expanded

        [PersonalData]
        public bool Pref8 { get; set; }

        [PersonalData]
        public bool Pref9 { get; set; }


        //[Display(Name = "Style Preferences")]
        //[StringLength(7000)]
        //[PersonalData]
        //public string? MyStyle { get; set; }

        [StringLength(100)]
        [PersonalData]
        public string? MyGuid { get; set; }

        //
        // Conversions between Db Entity space and gRPC space.
        //
        public static ApplicationUser GetApplicationUser(GAppUser other)
        {
            ApplicationUser u = new ApplicationUser();
            u.Id = other.Id;
            u.DisplayName = other.DisplayName;
            u.TimeZoneID = other.TimeZoneID;
            u.Ipref0 = other.Ipref0;
            u.Ipref1 = other.Ipref1;
            u.Ipref2 = other.Ipref2;
            u.Ipref3 = other.Ipref3;
            u.Ipref4 = other.Ipref4;
            u.Ipref5 = other.Ipref5;
            u.Ipref6 = other.Ipref6;
            u.Ipref7 = other.Ipref7;
            u.Ipref8 = other.Ipref8;
            u.Ipref9 = other.Ipref9;
            u.Pref0 = other.Pref0;
            u.Pref1 = other.Pref1;
            u.Pref2 = other.Pref2;
            u.Pref3 = other.Pref3;
            u.Pref4 = other.Pref4;
            u.Pref5 = other.Pref5;
            u.Pref6 = other.Pref6;
            u.Pref7 = other.Pref7;
            u.Pref8 = other.Pref8;
            u.Pref9 = other.Pref9;
            //u.MyGuid = other.MyGuid;  // null
            return u;
        }

        public static ApplicationUser MergeApplicationUser(GAppUser other, ApplicationUser u)
        {
            u.Id = other.Id;
            u.DisplayName = other.DisplayName;
            u.TimeZoneID = other.TimeZoneID;
            u.Ipref0 = other.Ipref0;
            u.Ipref1 = other.Ipref1;
            u.Ipref2 = other.Ipref2;
            u.Ipref3 = other.Ipref3;
            u.Ipref4 = other.Ipref4;
            u.Ipref5 = other.Ipref5;
            u.Ipref6 = other.Ipref6;
            u.Ipref7 = other.Ipref7;
            u.Ipref8 = other.Ipref8;
            u.Ipref9 = other.Ipref9;
            u.Pref0 = other.Pref0;
            u.Pref1 = other.Pref1;
            u.Pref2 = other.Pref2;
            u.Pref3 = other.Pref3;
            u.Pref4 = other.Pref4;
            u.Pref5 = other.Pref5;
            u.Pref6 = other.Pref6;
            u.Pref7 = other.Pref7;
            u.Pref8 = other.Pref8;
            u.Pref9 = other.Pref9;
            //u.MyGuid = other.MyGuid;  // null
            return u;
        }


        public GAppUser GetGAppUser()
        {
            GAppUser u = new GAppUser();
            u.Id = Id;
            u.DisplayName = DisplayName;
            u.TimeZoneID = TimeZoneID;
            u.Ipref0 = Ipref0;
            u.Ipref1 = Ipref1;
            u.Ipref2 = Ipref2;
            u.Ipref3 = Ipref3;
            u.Ipref4 = Ipref4;
            u.Ipref5 = Ipref5;
            u.Ipref6 = Ipref6;
            u.Ipref7 = Ipref7;
            u.Ipref8 = Ipref8;
            u.Ipref9 = Ipref9;
            u.Pref0 = Pref0;
            u.Pref1 = Pref1;
            u.Pref2 = Pref2;
            u.Pref3 = Pref3;
            u.Pref4 = Pref4;
            u.Pref5 = Pref5;
            u.Pref6 = Pref6;
            u.Pref7 = Pref7;
            u.Pref8 = Pref8;
            u.Pref9 = Pref9;
            //u.MyGuid = MyGuid;    //null
            return u;
        }

        public static List<ApplicationUser> GetApplicationUsers(GAppUserList other)
        {
            List<ApplicationUser> list = new List<ApplicationUser>();
            foreach (GAppUser user in other.List)
            {
                list.Add(GetApplicationUser(user));
            }
            return list;
        }

        public static GAppUserList GetGAppUserList(List<ApplicationUser> other)
        {
            GAppUserList list = new GAppUserList();
            foreach (ApplicationUser u in other)
            {
                list.List.Add(u.GetGAppUser());
            }
            return list;
        }
    }
}