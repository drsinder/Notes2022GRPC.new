namespace Notes2022.Server
{
    public static class Globals
    {
        public static string AccessOtherId { get; } = "Other";

        public static string ImportedAuthorId { get; } = "*imported*";

        public static string GuestId { get; set; } = "x";

        public static int TimeZoneDefaultID { get; set; } = 54;

        public static string ImportRoot { get; set; } = "E:\\Projects\\2022gRPC\\Notes2022GRPC\\Notes2022\\Server\\wwwroot\\Import\\";

        public static string SendGridEmail { get; set; } = "";

        public static string SendGridName { get; set; } = "";

        public static string SendGridApiKey { get; set; } = "";



        public static DateTime UTimeBlazor(DateTime dt)
        {
            //int OHours = TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).Hours;
            //int OMinutes = TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).Minutes;

            return dt; //.AddHours(-OHours).AddMinutes(-OMinutes);
        }

    }
}
