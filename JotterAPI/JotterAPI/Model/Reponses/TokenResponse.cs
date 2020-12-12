using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JotterAPI.Model.Reponses
{
    public class TokenResponse : ResponseResult
    {
        public string AccessToken { get; set; }

        public TokenResponse(string token)
        {
            AccessToken = token;
        }
    }
}
