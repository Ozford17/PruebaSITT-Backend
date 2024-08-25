using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend_SITT_Api.Aplication.Models.Identity
{
    public class JWTSettings
    {
        public string Key { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public TimeSpan ExpireTime { get; set; }
        public int RefreshTokenCharacterLenght { get; set; }
        public int RefreshTokenDurationInDays { get; set; }
    }
}
