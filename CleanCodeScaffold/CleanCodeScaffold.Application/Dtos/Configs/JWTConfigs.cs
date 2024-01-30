using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanCodeScaffold.Application.Dtos.Configs
{
    public class JWTConfigs
    {
        public JWTConfigs()
        {
            Issuer = string.Empty;
            Audience = string.Empty;
            Key = string.Empty;
        }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Key { get; set; }
    }
}
