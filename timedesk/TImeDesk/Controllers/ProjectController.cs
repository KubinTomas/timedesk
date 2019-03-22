using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using TImeDesk.Models;
using TImeDesk.Models.Database;
using TImeDesk.Models.Singletons;
using TImeDesk.ViewModel;

namespace TImeDesk.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {

        private readonly ApplicationDbContext _context;

        public ProjectController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Project
        public ActionResult Index()
        {
            var workSpaceId = WorkSpaceSingleton.Instance.Id;

            var userId = User.Identity.GetUserId<int>();

            var userProjects = _context.UserProjects
                .Where(p => p.WorkSpaceId == workSpaceId && p.ApplicationUserId == userId)
                .AsEnumerable()
                .Select(r => (int) r.ProjectId);
                
            

            var viewModel = new ProjectClientViewModel
            {
             
                Projects = _context.Projects
                                   .Include(p => p.WorkSpace)
                                   .Include(p => p.Client)
                                   .Include(s => s.Status)
                                   .Where(p => userProjects.Contains(p.Id) && p.StatusId == Status.Active).ToList(),

                Clients = _context.Clients.Where(p => p.WorkSpaceId == workSpaceId && p.StatusId == Status.Active).ToList()
            };

            SetViewBagData();

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
        public void AddProject(Project prj)
        {
            var workSpaceId = WorkSpaceSingleton.Instance.Id;
            var userId = User.Identity.GetUserId<int>();

            var project = new Project
            {
                Name = prj.Name,
                ClientId = prj.ClientId,
                WorkSpaceId = workSpaceId,
                StatusId = Status.Active,
                ApplicationUserId = userId,
                ColorString = prj.ColorString,
                Created = DateTime.Now,
                CurrencyId = _context.WorkSpaces.Single(c => c.Id == workSpaceId).CurrencyId

        };

            _context.Projects.Add(project);
            _context.SaveChanges();

            var userProject = new UserProject
            {
                ApplicationUserId = userId,
                WorkSpaceId = workSpaceId,
                ProjectId = project.Id,
            };

            _context.UserProjects.Add(userProject);
            _context.SaveChanges();

        }
        [HttpPost]
        public void DeleteProject(int id)
        {
            //Select project
            var project = _context.Projects.First(c => c.Id == id);

            //Select all tasks
            var tasks = _context.ProjectTasks.Where(c => c.ProjectId == id).ToList();

            //Set status deleted for each task
            tasks.ForEach(c => c.StatusId = Status.Deleted);

            //Set status deleted for main project
            project.StatusId = Status.Deleted;

            //Save changes
            _context.SaveChanges();
        }

        [HttpGet]
        public PartialViewResult GetProjectPartialViewResult()
        {
            var workSpaceId = WorkSpaceSingleton.Instance.Id;
            var userId = User.Identity.GetUserId<int>();

            var userProjects = _context.UserProjects
                .Where(p => p.WorkSpaceId == workSpaceId && p.ApplicationUserId == userId)
                .AsEnumerable()
                .Select(r => (int)r.ProjectId);

            var viewModel = new ProjectClientViewModel
            {
                Projects = _context.Projects
                    .Include(p => p.WorkSpace)
                    .Include(p => p.Client)
                    .Include(s => s.Status)
                    .Where(p => userProjects.Contains(p.Id) && p.StatusId == Status.Active).ToList(),

                Clients = _context.Clients.Where(p => p.WorkSpaceId == workSpaceId).ToList()
            };
            return PartialView("_ProjectsPartialView", viewModel);
        }
    }
}