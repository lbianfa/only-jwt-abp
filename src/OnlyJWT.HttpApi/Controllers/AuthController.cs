using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlyJWT.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Identity;

namespace OnlyJWT.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController: AbpControllerBase
    {

        private readonly IAuthAppService _authAppService;

        public AuthController(IAuthAppService authAppService)
        {
            _authAppService = authAppService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto input)
        {
            try
            {
                var result = await _authAppService.Login(input);

                return Ok(result);
            } catch (BusinessException ex) 
            {
                return Unauthorized(new { Message = ex.Code });
            }
        }
    }
}
