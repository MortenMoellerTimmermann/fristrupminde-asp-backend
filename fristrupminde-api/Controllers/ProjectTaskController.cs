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
using fristrupminde_api.Models.Outputs.Authentication;
using System.Web.Http.Cors;
using System.Collections.Generic;
using fristrupminde_api.Services;
using fristrupminde_api.Models.Outputs.ProjectTaskOutputs;

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
                    List<ProjectTask> tasks = await _context.ProjectTasks.Where(task => task.CompletedDate == null).ToListAsync();
                    tasks = tasks.Where(task => taskIDs.Contains(task.ID)).ToList();
                    return Json(tasks);
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
        [Route("api/task/user/assign")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<IActionResult> assignTaskToUser([FromBody] AssignUserInput assignUserInput)
        {
            string token = HttpContext.Request.Headers["Authorization"];
            if (token != null)
            {
                try
                {
                    ProjectTask task = await _context.ProjectTasks.SingleOrDefaultAsync(x => x.ID == new Guid(assignUserInput.taskID));
                    if(task == null)
                    {
                        return NotFound();
                    }

                    ApplicationUser user = await _userManager.FindByEmailAsync(_jwtService.GetClaimValue(token, "email"));
                    ProjectTaskUser NewPTU = new ProjectTaskUser();
                    NewPTU.ProjectTaskID = task.ID;
                    NewPTU.UserID = user.Id;
                    _context.Add(NewPTU);
                    await _context.SaveChangesAsync();
                    return Ok();
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

        [HttpPost]
        [Route("api/task/create")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<IActionResult> createTask([FromBody]CreateTaskInput taskInput)
        {
            string token = HttpContext.Request.Headers["Authorization"];
            if (token != null)
            {
                try
                {
                    ApplicationUser user = await _userManager.FindByEmailAsync(_jwtService.GetClaimValue(token, "email"));
                    ProjectTask NewTask = new ProjectTask();
                    NewTask.Title = taskInput.Title;
                    NewTask.Description = taskInput.Description;
                    NewTask.Created = DateTime.Now;
                    NewTask.DueDate = DateTime.Parse(taskInput.DueDate);
                    NewTask.CreatedBy = user.Email;

                    _context.Add(NewTask);
                    if (!string.IsNullOrEmpty(taskInput.AssignedTo))
                    {
                        //TODO split string by ; and make multiple PTU
                        ApplicationUser AssignTo = await _userManager.FindByEmailAsync(taskInput.AssignedTo);
                        ProjectTaskUser NewPTU = new ProjectTaskUser();
                        NewPTU.ProjectTaskID = NewTask.ID;
                        NewPTU.UserID = AssignTo.Id;
                        _context.Add(NewPTU);
                    }

                    await _context.SaveChangesAsync();

                    return Json(NewTask.ID);
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


        [HttpPost]
        [Route("api/task/details")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<IActionResult> taskDetails([FromBody] DetailsTaskInput detailsTaskInput)
        {
            string token = HttpContext.Request.Headers["Authorization"];
            if (token != null)
            {
                try
                {

                    Guid taskIDAsGuid = new Guid(detailsTaskInput.taskID);
                    DetailsTaskOutput DTO = new DetailsTaskOutput();


                    ProjectTask task = await _context.ProjectTasks.SingleOrDefaultAsync(x => x.ID == taskIDAsGuid);
                    if (task == null)
                    {
                        return NotFound();
                    }

                    ApplicationUser user = await _userManager.FindByEmailAsync(_jwtService.GetClaimValue(token, "email"));

                    //Gets all the users on the task
                    List<Guid> userIDs = await _context.ProjectTaskUsers.Where(UT => UT.ProjectTaskID == taskIDAsGuid).Select(UT => UT.UserID).ToListAsync();
                    List<ApplicationUser> UsersOnTask = await _context.Users.ToListAsync();
                    UsersOnTask = UsersOnTask.Where(u => userIDs.Contains(u.Id)).ToList();
                    DTO.UsersOnTask = new List<UserOutput>();
                    UsersOnTask.ForEach(u =>
                    {
                        UserOutput UO = new UserOutput();
                        UO.Username = u.UserName;
                        UO.Email = u.Email;
                        UO.Avatar = "https://www.suitdoctors.com/wp-content/uploads/2016/11/dummy-man-570x570.png";
                        DTO.UsersOnTask.Add(UO);
                    });

                    DTO.Remarks = await _context.Remarks.Where(Remark => Remark.ProjectTaskID == taskIDAsGuid).ToListAsync();
                    return Json(DTO);
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

        [HttpPut]
        [Route("api/task/finish")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<IActionResult> finishTask([FromBody]FinishTaskInput finishTaskInput)
        {
            string token = HttpContext.Request.Headers["Authorization"];
            if (token != null)
            {
                try
                {
                    ProjectTask task = await _context.ProjectTasks.SingleOrDefaultAsync(x => x.ID == new Guid(finishTaskInput.taskID));
                    if (task == null)
                    {
                        return NotFound();
                    }

                    ApplicationUser user = await _userManager.FindByEmailAsync(_jwtService.GetClaimValue(token, "email"));
                    task.CompletedDate = DateTime.Now;
                    task.CompletedBy = user.Email;
                    //Maybe delete ProjectTaskUser object for the task or keep it for seeing previous completed tasks
                    _context.Update(task);
                    await _context.SaveChangesAsync();
                    return Ok();
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
