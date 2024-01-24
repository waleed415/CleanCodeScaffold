using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanCodeScaffold.Application.Dtos
{
    public class TokenVM
    {
        public TokenVM()
        {
            Token = string.Empty;
            RefreshToken = string.Empty;
            Name = string.Empty;
            Roles = new List<string>();
        }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string Name { get; set; }
        public List<string> Roles { get; set; }
    }
}
