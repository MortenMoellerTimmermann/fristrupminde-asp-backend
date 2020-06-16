using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using fristrupminde_api.Models;
using fristrupminde_api.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using fristrupminde_api.Models.Inputs.Statistics;
using fristrupminde_api.Models.Outputs.Statistics;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System;
using System.Security.Claims;
using System.Text;
using System.Web.Http.Cors;
using System.Collections.Generic;
using fristrupminde_api.Services;

namespace fristrupminde_api.Controllers
{
    public class StatisticsController : Controller
    {
        private IConfiguration _config;
        private readonly DataContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private JwtService _jwtService;

        public StatisticsController(IConfiguration config, UserManager<ApplicationUser> userManager, DataContext dataContext)
        {
            this._config = config;
            this._userManager = userManager;
            this._context = dataContext;
            _jwtService = new JwtService(_config);
        }


        [HttpPost]
        [Route("api/statistics/create")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<IActionResult> createStatisticsData([FromBody]CreateStatisticsData createStatisticsData)
        {
            string token = HttpContext.Request.Headers["Authorization"];
            if (token != null)
            {
                try
                {
                    ApplicationUser user = await _userManager.FindByEmailAsync(_jwtService.GetClaimValue(token, "email"));
                    StatisticsData SD = new StatisticsData();
                    SD.MilkLiter = createStatisticsData.MilkLiter;
                    SD.FatPercentage = createStatisticsData.FatPercentage;
                    SD.DateForData = DateTime.Parse(createStatisticsData.DateForData);
                    SD.CreatedBy = user.Email;

                    _context.Add(SD);
                    await _context.SaveChangesAsync();
                    return Json(SD.ID);
                }
                catch (ApplicationException e)
                {
                    return Unauthorized();
                }
                catch (Exception e)
                {
                    return NotFound();
                }
            }

            return Unauthorized();
        }

        [HttpGet]
        [Route("api/statistics/get")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<IActionResult> getStatisticsData()
        {
            string token = HttpContext.Request.Headers["Authorization"];
            if (token != null)
            {
                try
                {
                    ApplicationUser user = await _userManager.FindByEmailAsync(_jwtService.GetClaimValue(token, "email"));
                    List<StatisticsData> SDFromDB = await _context.StatisticsDatas.ToListAsync();
                    SDFromDB = SDFromDB.OrderBy(SD => SD.DateForData).ToList();
                    return Json(SDFromDB);
                }
                catch (ApplicationException e)
                {
                    return Unauthorized();
                }
                catch (Exception e)
                {
                    return NotFound();
                }
            }

            return Unauthorized();
        }
    }
}
