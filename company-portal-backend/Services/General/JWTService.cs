using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Services.General.Interface;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services.General
{
    public class JWTService : IJWTService
    {
        private readonly IConfiguration _config;

        public JWTService(IConfiguration config)
        {
            _config = config;
        }
        public string GenerateToken(DAL.Models.Company model)
        {
            var userClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("Id", model.Id.ToString()),
                new Claim("Email", model.Email),
                new Claim("Name", model.EnglishName),
                //new Claim("IsEmailVerified", model.IsEmailVerified.ToString().ToLower())
            };

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SecurityKey"]));
            var token = new JwtSecurityToken(
                expires: DateTime.Now.AddMinutes(10),
                claims: userClaims,
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
