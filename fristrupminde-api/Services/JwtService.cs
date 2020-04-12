using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using fristrupminde_api.Models;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace fristrupminde_api.Services
{
    public class JwtService
    {
        private readonly IConfiguration _config;

        public JwtService(IConfiguration config)
        {
            this._config = config;
        }

        public string GenerateJwtToken(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_config["Jwt:ExpireDays"]));

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GetClaimValue(string token, string claimType)
        {
            if (ValidateToken(token))
            {
                JwtSecurityToken readToken = ReadToken(token);
                return readToken.Claims.FirstOrDefault(claim => claim.Type == claimType).Value;
            }
            throw new ApplicationException("Claim not found");
        }

        public JwtSecurityToken ReadToken(string token)
        {
            return new JwtSecurityTokenHandler().ReadJwtToken(token);
        }

        public bool ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = GetValidationParameters();

            SecurityToken validatedToken;
            try
            {
                tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
            }
            catch
            {
                return false;
            }
            return true;
        }

        private TokenValidationParameters GetValidationParameters()
        {
            return new TokenValidationParameters()
            {
                ValidIssuer = _config["Jwt:Issuer"],
                ValidAudience = _config["Jwt:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"])),
                ClockSkew = TimeSpan.Zero // remove delay of token when expire generate the token
            };
        }

    }
}
