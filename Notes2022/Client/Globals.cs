using Grpc.Core;
using Notes2022.Proto;
using Notes2022.Client.Shared;
using Notes2022.Client.Menus;
using Notes2022.Client.Pages.Admin;
using Notes2022.Client.Pages;

namespace Notes2022.Client
{
    public static class Globals
    {
        public static LoginDisplay? LoginDisplay = null;
        public static NavMenu? NavMenu = null;
        public static NotesFilesAdmin? NotesFilesAdmin = null;
        public static string AccessOtherId { get; } = "Other";

        public static string Cookie { get; } = "notes2022login";

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

    }
}
