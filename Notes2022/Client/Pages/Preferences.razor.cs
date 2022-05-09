using Notes2022.Proto;
using Notes2022.Shared;

namespace Notes2022.Client.Pages
{
    public partial class Preferences
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private GAppUser UserData { get; set; }

        private string currentText { get; set; }

        private List<LocalModel2> MySizes { get; set; }

        private string pageSize { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        protected override async Task OnInitializedAsync()
        {
            UserData = await Client.GetUserDataAsync(new NoRequest(), myState.AuthHeader);
            pageSize = UserData.Ipref2.ToString();
            MySizes = new List<LocalModel2> { new LocalModel2("0", "All"), new LocalModel2("5"), new LocalModel2("10"), new LocalModel2("12"), new LocalModel2("20") };
            currentText = " ";
        }

        private async Task OnSubmit()
        {
            UserData.Ipref2 = int.Parse(pageSize);
            await Client.UpdateUserDataAsync(UserData, myState.AuthHeader);
            Navigation.NavigateTo("");
        }

        private void Cancel()
        {
            Navigation.NavigateTo("");
        }

        public class LocalModel2
        {
            public LocalModel2(string psize)
            {
                Psize = psize;
                Name = psize;
            }

            public LocalModel2(string psize, string name)
            {
                Psize = psize;
                Name = name;
            }

            public string Psize { get; set; }

            public string Name { get; set; }
        }
    }
}