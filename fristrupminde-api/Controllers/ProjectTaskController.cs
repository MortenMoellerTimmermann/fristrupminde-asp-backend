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

namespace fristrupminde_api.Controllers
{
    public class ProjectTaskController : Controller
    {
        private IConfiguration _config;
        private readonly DataContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProjectTaskController(IConfiguration config, UserManager<ApplicationUser> userManager, DataContext dataContext)
        {
            this._config = config;
            this._userManager = userManager;
            this._context = dataContext;
        }

        [HttpGet]
        [Route("api/getTasks")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<IActionResult> getTasks()
        {
            List<ProjectTask> task = await _context.ProjectTasks.ToListAsync();

            return Json(task);
        }

        [HttpPost]
        [Route("api/postTask")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<IActionResult> postTask([FromBody]CreateTaskInput taskInput)
        {
            var user = await _userManager.GetUserAsync(User);
            ProjectTask NewTask = new ProjectTask();
            NewTask.Title = taskInput.Title;
            NewTask.Description = taskInput.Description;
            NewTask.Created = DateTime.Now;
            NewTask.DueDate = DateTime.Parse(taskInput.DueDate);

            _context.Add(NewTask);
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
