using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using fristrupminde_api.Models.Inputs.Authentication;
using System.Linq;
using fristrupminde_api.Models;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Web.Http.Cors;
using Microsoft.IdentityModel.Tokens;
using System.Security.Principal;

namespace fristrupminde_api.Controllers
{
    public class AuthenticationController : Controller
    {

        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;

        public AuthenticationController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = configuration;
        }

        [HttpPost]
        [Route("api/login")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<Object> Login([FromBody] LoginDTO model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

            if (result.Succeeded)
            {
                ApplicationUser user = _userManager.Users.SingleOrDefault(r => r.Email == model.Email);
                return await GenerateJwtToken(user);
            }

            throw new ApplicationException("INVALID_LOGIN_ATTEMPT");
        }

        [HttpPost]
        [Route("api/register")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<Object> Register([FromBody] RegisterDTO model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email
            };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return await GenerateJwtToken(user);
            }

            throw new ApplicationException("UNKNOWN_ERROR");
        }


        [Route("api/user/validate")]
        [HttpPost]
        [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
        public async Task<IActionResult> ValidateUserToken(string token)
        {
            if (token != null)
            {
                if (ValidateToken(token))
                {
                    return Ok();
                }
            }
            return Unauthorized();
        }


        private async Task<Object> GenerateJwtToken(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.UserName)
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


        private bool ValidateToken(string token)
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
