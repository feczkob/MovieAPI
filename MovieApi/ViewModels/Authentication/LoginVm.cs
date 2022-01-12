using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApi.ViewModels.Authentication
{
    public class LoginVm
    {
        public string Token { get; set; }
        public DateTime Expires { get; set; }
    }
}
