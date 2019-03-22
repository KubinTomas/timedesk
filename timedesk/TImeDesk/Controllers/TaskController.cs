using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using TImeDesk.Models;
using TImeDesk.Models.Database;
using TImeDesk.Models.Singletons;
using TImeDesk.ViewModel;

namespace TImeDesk.Controllers
{
    public class TaskController : Controller
    {
        private ApplicationDbContext _context;

        public TaskController()
        {
            _context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        // GET: Task
        public ActionResult Index()
        {
            SetViewBagData();

            var workSpaceId = WorkSpaceSingleton.Instance.Id;

            var userId = User.Identity.GetUserId<int>();

            var userProjects = _context.UserProjects
                .Where(p => p.WorkSpaceId == workSpaceId && p.ApplicationUserId == userId)
                .AsEnumerable()
                .Select(r => (int)r.ProjectId);

            var viewModel = new TaskProjectViewModel
            {
                Projects = _context.Projects.Where(p => userProjects.Contains(p.Id) && p.StatusId == Status.Active).ToList(),
                Tasks = _context.ProjectTasks.Where(c => c.WorkSpaceId == workSpaceId && c.StatusId == Status.Active && userProjects.Contains( c.ProjectId)).Include(s => s.Status).ToList(),
                AllProjects = _context.Projects.Where(c => c.WorkSpaceId == workSpaceId),

            };
            return View(viewModel);
        }

        public void SetViewBagData()
        {
            var currentUserId = User.Identity.GetUserId<int>();

            // User is signed
            if (currentUserId == 0) return;

            var currentWorkspaceId = _context.UserSettingses.Single(c => c.ApplicationUserId == currentUserId).currentWorkspace;
            var permissionId = _context.UserWorkspace.Single(c => c.ApplicationUserId == currentUserId && c.WorkspaceId == currentWorkspaceId).UserWorkspaceRolesId;

            ViewBag.UserWorkspaceRoles = _context.UserWorkspaceRoles.Single(c => c.Id == permissionId);
        }

        [HttpPost]
        public void DeleteTask(int id)
        {

            var task = _context.ProjectTasks.First(c => c.Id == id);

            task.StatusId = Status.Deleted;

            _context.SaveChanges();

        }
        [HttpPost]
        public void AddTask(ProjectTask tsk)
        {
           
            if(tsk.ProjectId == 0) 
                return;

            var workSpaceId = WorkSpaceSingleton.Instance.Id;

            var task = new ProjectTask()
            {
                Name = tsk.Name,
                ProjectId = tsk.ProjectId,
                WorkSpaceId = workSpaceId,
                StatusId = Status.Active,
                ColorString = tsk.ColorString,


            };

            _context.ProjectTasks.Add(task);
            _context.SaveChanges();

        }
        [HttpGet]
        public PartialViewResult GetTaskPartialViewResult()
        {
            var workSpaceId = WorkSpaceSingleton.Instance.Id;


            var userId = User.Identity.GetUserId<int>();

            var userProjects = _context.UserProjects
                .Where(p => p.WorkSpaceId == workSpaceId && p.ApplicationUserId == userId)
                .AsEnumerable()
                .Select(r => (int)r.ProjectId);

            var viewModel = new TaskProjectViewModel
            {
                Projects = _context.Projects.Where(p => userProjects.Contains(p.Id) && p.StatusId == Status.Active).ToList(),
                Tasks = _context.ProjectTasks.Where(c => c.WorkSpaceId == workSpaceId && c.StatusId == Status.Active)
                                             .Include(s => s.Status).ToList(),
                AllProjects = _context.Projects.Where(c =>c.WorkSpaceId == workSpaceId),


            };
            return PartialView("_TasksPartialView", viewModel);
        }
    }
}