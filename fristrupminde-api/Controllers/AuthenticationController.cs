using System;
using fristrupminde_api.Models.Inputs.Authentication;
using fristrupminde_api.Services;
using System.Linq;
using fristrupminde_api.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Web.Http.Cors;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Collections.Generic;
using fristrupminde_api.Data;

namespace fristrupminde_api.Controllers
{
    public class AuthenticationController : Controller
    {

        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;
        private readonly DataContext _context;
        private JwtService _jwtService;

        public AuthenticationController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration, DataContext dataContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = configuration;
            _context = dataContext;
            _jwtService = new JwtService(_config);
        }

        [HttpPost]
        [Route("api/user/login")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

            if (result.Succeeded)
            {
                ApplicationUser user = _userManager.Users.SingleOrDefault(r => r.Email == model.Email);
                return Json(_jwtService.GenerateJwtToken(user));
            }

            return NotFound();
        }

        [HttpPost]
        [Route("api/user/register")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
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
                return Json(_jwtService.GenerateJwtToken(user));
            }

            return NotFound();
        }

        [HttpGet]
        [Route("api/user/emails")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<IActionResult> getUserEmails()
        {
            string token = HttpContext.Request.Headers["Authorization"];
            if (token != null)
            {
                if (_jwtService.ValidateToken(token))
                {
                    List<string> emails = await _context.Users.Select(user => user.Email).ToListAsync();
                    return Json(emails);
                }
            }
            return Unauthorized();
        }


        [Route("api/user/validate")]
        [HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<IActionResult> ValidateUserToken()
        {
            string token = HttpContext.Request.Headers["Authorization"];
            if (token != null)
            {
                try
                {
                    ApplicationUser user = await _userManager.FindByEmailAsync(_jwtService.GetClaimValue(token, "email"));
                    return Json(user.Email);
                }
                catch
                {
                    return Unauthorized();
                }
            }
            return Unauthorized();
        }

    }
}
