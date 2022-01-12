using System;

namespace MovieApi.ViewModels.Authentication
{
    public class LoginVm
    {
        public string Token { get; set; }
        public DateTime Expires { get; set; }
    }
}
