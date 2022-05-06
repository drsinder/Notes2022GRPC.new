using Grpc.Core;
using Notes2022.Proto;
using Notes2022.Client.Shared;
using Notes2022.Client.Menus;
using Notes2022.Client.Pages.Admin;
using Notes2022.Client.Pages;
using System.Text;

namespace Notes2022.Client
{
    public static class Globals
    {
        public static LoginDisplay? LoginDisplay = null;
        public static NavMenu? NavMenu = null;
        public static NotesFilesAdmin? NotesFilesAdmin = null;
        public static string AccessOtherId { get; } = "Other";

        public static string Cookie { get; } = "notes2022login";

        public static string returnUrl { get; set; } = string.Empty;


        public static DateTime LocalTimeBlazor(DateTime dt)
        {
            int OHours = TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).Hours;
            int OMinutes = TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).Minutes;

            return dt.AddHours(OHours * 2).AddMinutes(OMinutes * 2);    // *2 needed because we go in and out of unix utc time
        }

        public static DateTime UTimeBlazor(DateTime dt)
        {
            int OHours = TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).Hours;
            int OMinutes = TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).Minutes;

            return dt.AddHours(-OHours).AddMinutes(-OMinutes);    // *2 needed because we go in and out of unix utc time
        }


        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string encodedString)
        {
            byte[] data = Convert.FromBase64String(encodedString);
            string decodedString = Encoding.UTF8.GetString(data);
            return decodedString;
        }

    }
}
