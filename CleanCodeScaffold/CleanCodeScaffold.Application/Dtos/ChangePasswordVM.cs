using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanCodeScaffold.Application.Dtos
{
    public class ChangePasswordVM
    {
        public ChangePasswordVM()
        {
            OldPassword = string.Empty;
            NewPassword = string.Empty;
        }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
