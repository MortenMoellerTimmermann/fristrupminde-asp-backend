using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using fristrupminde_api.Models;
using fristrupminde_api.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using fristrupminde_api.Models.Inputs.ProjectTaskInputs;
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
    public class ProjectTaskController : Controller
    {
        private IConfiguration _config;
        private readonly DataContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private JwtService _jwtService;

        public ProjectTaskController(IConfiguration config, UserManager<ApplicationUser> userManager, DataContext dataContext)
        {
            this._config = config;
            this._userManager = userManager;
            this._context = dataContext;
            _jwtService = new JwtService(_config);
        }

        [HttpGet]
        [Route("api/getAllTasks")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<IActionResult> getTasks()
        {
            List<ProjectTask> task = await _context.ProjectTasks.ToListAsync();

            return Json(task);
        }


        [HttpGet]
        [Route("api/user/getTasks")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<IActionResult> getUserTasks()
        {
            string token = HttpContext.Request.Headers["Authorization"];
            if (token != null)
            {
                try
                {
                    ApplicationUser user = await _userManager.FindByEmailAsync(_jwtService.GetClaimValue(token, "email"));
                    List<Guid> taskIDs = await _context.ProjectTaskUsers.Where(UT => UT.UserID == user.Id).Select(UT => UT.ProjectTaskID).ToListAsync();
                    List<ProjectTask> tasks = await _context.ProjectTasks.ToListAsync();
                    tasks = tasks.Where(task => taskIDs.Contains(task.ID)).ToList();
                    return Json(tasks);
                }
                catch(ApplicationException e)
                {
                    return Unauthorized();
                }
                catch(Exception e)
                {
                    return NotFound();
                }
            }

            return Unauthorized();
        }

        [HttpPost]
        [Route("api/createTask")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<IActionResult> createTask([FromBody]CreateTaskInput taskInput)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(taskInput.AssignedTo);
            ProjectTask NewTask = new ProjectTask();
            NewTask.Title = taskInput.Title;
            NewTask.Description = taskInput.Description;
            NewTask.Created = DateTime.Now;
            NewTask.DueDate = DateTime.Parse(taskInput.DueDate);

            _context.Add(NewTask);

            ProjectTaskUser NewPTU = new ProjectTaskUser();
            NewPTU.ProjectTaskID = NewTask.ID;
            NewPTU.UserID = user.Id;
            _context.Add(NewPTU);

            await _context.SaveChangesAsync();

            return Json(NewTask.ID);
        }

        [HttpDelete]
        [Route("api/deleteTask/{id}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<IActionResult> deleteTask(Guid id)
        {
            ProjectTask InputTask = new ProjectTask();

            ProjectTask PT = await _context.ProjectTasks.SingleOrDefaultAsync(x => x.ID == id);
            if (PT == null)
            {
                return NotFound();
            }
            _context.ProjectTasks.Remove(PT);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
