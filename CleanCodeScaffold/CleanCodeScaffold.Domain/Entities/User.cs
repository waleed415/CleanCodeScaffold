using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanCodeScaffold.Domain.Entities
{
    public class User : IdentityUser<long>
    {
        public User()
        {
            RefreshToken = string.Empty;
        }
        public string RefreshToken { get; set; }

        public ICollection<UserToken> Tokens { get; } = new List<UserToken>();
    }
}
