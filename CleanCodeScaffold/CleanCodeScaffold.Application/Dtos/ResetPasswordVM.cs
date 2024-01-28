using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanCodeScaffold.Application.Dtos
{
    public class ResetPasswordVM
    {
        public ResetPasswordVM()
        {
            Token = string.Empty;
            EmailorPhone = string.Empty;
            Password = string.Empty;
        }
        public string Token { get; set; }
        public string EmailorPhone { get; set; }
        public string Password { get; set; }
    }
}
