using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;
using fristupmindeAPI.Models;
using fristupmindeAPI.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Web.Http.Cors;
using System.Collections.Generic;



namespace fristupmindeAPI.Controllers
{
    public class TaskController : Controller
    {
        private IConfiguration _config;
        private readonly DataContext _context;

        public TaskController(IConfiguration config, DataContext dataContext)
        {
            this._config = config;
            this._context = dataContext;
        }

        [HttpGet]
        [Route("api/getTasks")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<IActionResult> getTasks()
        {
            List<ProjectTask> task = await _context.Tasks.ToListAsync();

            return Json(task);
        }

        [HttpPost]
        [Route("api/postTask")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<IActionResult> postTask([FromBody] ProjectTask task)
        {
            ProjectTask InputTask = new ProjectTask();
            InputTask.ProjectTaskID = 20;
            InputTask.Title = task.Title;
            InputTask.Description = task.Description;

            _context.Add(InputTask);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete]
        [Route("api/deleteTask/{id}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<IActionResult> postTask(int id)
        {
            ProjectTask InputTask = new ProjectTask();

            ProjectTask PT = await _context.Tasks.SingleOrDefaultAsync(x => x.ProjectTaskID == id);
            if(PT == null)
            {
                return NotFound();
            }
            _context.Tasks.Remove(PT);
            await _context.SaveChangesAsync();
            return Ok();
        }


    }
}
