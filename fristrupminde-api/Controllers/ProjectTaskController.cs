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
        [Route("api/task/getAll")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<IActionResult> getTasks()
        {
            List<ProjectTask> task = await _context.ProjectTasks.ToListAsync();

            return Json(task);
        }


        [HttpGet]
        [Route("api/task/user/getTasks")]
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

        [HttpGet]
        [Route("api/task/getAvailable")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<IActionResult> getAvailableTasks()
        {
            string token = HttpContext.Request.Headers["Authorization"];
            if (token != null)
            {
                if (_jwtService.ValidateToken(token))
                {
                    List<Guid> taskIDs = await _context.ProjectTaskUsers.Select(UT => UT.ProjectTaskID).ToListAsync();
                    List<ProjectTask> tasks = await _context.ProjectTasks.ToListAsync();
                    //Does not contain - means available. Might need to be changed when multiple person can have one task
                    tasks = tasks.Where(task => !taskIDs.Contains(task.ID)).ToList();
                    return Json(tasks);
                }
            }

            return Unauthorized();
        }

        [HttpPost]
        [Route("api/task/create")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<IActionResult> createTask([FromBody]CreateTaskInput taskInput)
        {
            ProjectTask NewTask = new ProjectTask();
            NewTask.Title = taskInput.Title;
            NewTask.Description = taskInput.Description;
            NewTask.Created = DateTime.Now;
            NewTask.DueDate = DateTime.Parse(taskInput.DueDate);

            _context.Add(NewTask);
            if (!string.IsNullOrEmpty(taskInput.AssignedTo))
            {
                ApplicationUser user = await _userManager.FindByEmailAsync(taskInput.AssignedTo);
                ProjectTaskUser NewPTU = new ProjectTaskUser();
                NewPTU.ProjectTaskID = NewTask.ID;
                NewPTU.UserID = user.Id;
                _context.Add(NewPTU);
            }

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
