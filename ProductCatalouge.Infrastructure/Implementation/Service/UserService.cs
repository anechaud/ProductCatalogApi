using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProductCatalouge.Core.Configuration;
using ProductCatalouge.Core.Interfaces;
using ProductCatalouge.Core.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalouge.Infrastructure.Implementation.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private IOptionsSnapshot<GlobalConfig> _config { get; set; }
        public UserService(IUserRepository userRepository, IOptionsSnapshot<GlobalConfig> config)
        {
            _config = config;
            _userRepository = userRepository;
        }

        public async Task<string> AuthenticateAsync(string username, string password)
        {
            var user = await _userRepository.GetUser(username, password);

            if (user == null)
                return null;
            var roles = await _userRepository.GetRoles(username);
            var claims = new List<Claim>
            {
                new Claim("username", user.UserId),
                new Claim("email", user.Email)
            };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.RoleName));
            }

            var token = GetJwtToken(user.UserId, TimeSpan.FromMinutes(60), claims.ToArray());
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private JwtSecurityToken GetJwtToken(string username, TimeSpan expiration, Claim[] additionalClaims = null)
        {
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub,username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            if (additionalClaims is object)
            {
                var claimList = new List<Claim>(claims);
                claimList.AddRange(additionalClaims);
                claims = claimList.ToArray();
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Value.Authentication.Secret)); // make configureable TODO
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            return new JwtSecurityToken(
                expires: DateTime.UtcNow.Add(expiration),
                claims: claims,
                signingCredentials: creds
            );
        }
    }
}
