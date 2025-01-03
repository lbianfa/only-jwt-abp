using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Identity;

namespace OnlyJWT.Auth
{
    public interface IAuthAppService
    {
        Task<LoginResultDto> Login(LoginDto input);
    }
}
