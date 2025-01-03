using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using IdentityUser = Volo.Abp.Identity.IdentityUser;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace OnlyJWT.Auth
{
    [RemoteService(false)]
    public class AuthAppService : ApplicationService, IAuthAppService
    {
        private readonly IRepository<IdentityUser> _userRepository;
        private readonly IdentityUserManager _userManager;
        private readonly IConfiguration _configuration;

        public AuthAppService(IRepository<IdentityUser> userRepository, IdentityUserManager userManager, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _configuration = configuration;

        }

        public async Task<LoginResultDto> Login(LoginDto input)
        {
            var user = await _userRepository.FindAsync(u => u.UserName.Equals(input.Username.ToLower()));
            if (user == null)
            {
                //throw new AbpAuthorizationException();
                throw new BusinessException("Invalid username or password");
            }

            var loggedIn = await _userManager.CheckPasswordAsync(user, input.Password);
            if (!loggedIn)
            {
                throw new BusinessException("Invalid username or password");
            }

            return GenerateJwtToken(user);
        }

        private LoginResultDto GenerateJwtToken(IdentityUser user)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtBearer:SecurityKey"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
            };

                var roles = _userManager.GetRolesAsync(user).Result;
                claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

                DateTime currentDate = DateTime.Now;
                DateTime expireDate = currentDate.AddDays(1);
                TimeSpan difference = expireDate - currentDate;
                double expireInSeconds = difference.TotalSeconds;

                var token = new JwtSecurityToken(
                    issuer: _configuration["JwtBearer:Issuer"],
                    audience: _configuration["JwtBearer:Audience"],
                    claims: claims,
                    expires: expireDate,
                    signingCredentials: credentials
                );

                return new LoginResultDto
                {
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                    ExpireInSeconds = expireInSeconds,
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
