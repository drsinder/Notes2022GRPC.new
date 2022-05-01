using Grpc.Core;
using Notes2022.Proto;
using Google.Protobuf;

namespace Notes2022.Client
{
    public class StateContainer
    {
        private LoginReply? savedLogin;

        public LoginReply? LoginReply
        {
            get 
            {
                return savedLogin;  
            }
            set
            {
                savedLogin = value;
                NotifyStateChanged();
            }
        }

        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();

        public bool IsAuthenticated
        {
            get
            {
                return LoginReply != null && LoginReply.Status == 200;
            }
        }

        public bool IsAdmin
        {
            get
            {
                if (LoginReply == null || LoginReply.Status !=200)
                    return false;
                return UserInfo.IsAdmin;
            }
        }

        public bool IsUser
        {
            get
            {
                if (LoginReply == null || LoginReply.Status != 200)
                    return false;
                return UserInfo.IsUser;
            }
        }

        public Metadata AuthHeader
        {
            get
            {
                var headers = new Metadata();
                if (LoginReply != null && LoginReply.Status == 200)
                    headers.Add("Authorization", $"Bearer {LoginReply.Jwt}");
                return headers;
            }
        }

        public UserInfo? UserInfo
        {
            get
            {
                if (LoginReply != null && LoginReply.Status == 200)
                {
  
                    return LoginReply.Info;
                }

                return null;
            }
        }
    }
}
