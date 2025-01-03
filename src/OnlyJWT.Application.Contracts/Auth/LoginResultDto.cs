using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlyJWT.Auth
{
    public class LoginResultDto
    {
        public string AccessToken { get; set; }
        public double ExpireInSeconds { get; set; }
    }
}
