using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanCodeScaffold.Application.Dtos
{
    public class LoginVM
    {
        public LoginVM()
        {
            UserName = string.Empty;
            Password = string.Empty;
        }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
