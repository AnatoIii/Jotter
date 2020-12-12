using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JotterAPI.Helpers
{
    public class TokenConfig
    {
        public int JWTLifetime { get; set; }
        public string Secret { get; set; }
        public string Issuer { get; set; }
    }
}
