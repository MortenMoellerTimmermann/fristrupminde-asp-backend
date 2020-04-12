using System;
using fristrupminde_api.Models.Inputs.Authentication;
using fristrupminde_api.Services;
using System.Linq;
using fristrupminde_api.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Web.Http.Cors;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;

namespace fristrupminde_api.Controllers
{
    public class AuthenticationController : Controller
    {

        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;
        private JwtService _jwtService;

        public AuthenticationController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = configuration;
            _jwtService = new JwtService(_config);
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
                return await _jwtService.GenerateJwtToken(user);
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
                return await _jwtService.GenerateJwtToken(user);
            }

            throw new ApplicationException("UNKNOWN_ERROR");
        }


        [Route("api/user/validate")]
        [HttpPost]
        [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
        public async Task<IActionResult> ValidateUserToken()
        {
            string token = HttpContext.Request.Headers["Authorization"];
            if (token != null)
            {
                if (_jwtService.ValidateToken(token))
                {
                    JwtSecurityToken readToken =_jwtService.ReadToken(token);
                    string email = readToken.Claims.FirstOrDefault(claim => claim.Type == "email").Value;
                    ApplicationUser user = await _userManager.FindByEmailAsync(email);
                    return Json("Token is validated for user: " + user.Email);
                }
            }
            return Unauthorized();
        }



    }
}
