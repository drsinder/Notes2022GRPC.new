using System.Text;

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


        public static DateTime UTimeBlazor(DateTime dt)
        {
            //int OHours = TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).Hours;
            //int OMinutes = TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).Minutes;

            return dt; //.AddHours(-OHours).AddMinutes(-OMinutes);
        }

    }
}
