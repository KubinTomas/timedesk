using AutoMapper;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TImeDesk.Models;
using TImeDesk.Models.Database;
using TImeDesk.Models.Others;
using TImeDesk.Models.Singletons;
using TImeDesk.ViewModel;
using TImeDesk.ViewModel.Workspace.Permissions;
using TImeDesk.ViewModel.Workspace.User;


namespace TImeDesk.Controllers
{
    public class WorkspaceController : Controller
    {

        private ApplicationDbContext _context;
        // GET: Workspace
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId<int>();


            var workspaces = _context.UserWorkspace.Where(c => c.ApplicationUserId == userId && c.StatusId == Status.Accepted).Include(c => c.WorkSpace);

            var workspaceInvitations = _context.UserWorkspace.Where(c => c.ApplicationUserId == userId && c.StatusId == Status.Pending).Include(c => c.WorkSpace);


            var viewModel = new WorkspaceViewModel
            {
                WorkSpaces = workspaces,
                WorkspaceInvitations = workspaceInvitations,
                currentWorkspaceId = WorkSpaceSingleton.Instance.Id,

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

        public WorkspaceController()
        {
            _context = new ApplicationDbContext();


        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        public ActionResult CreateWorkspaceView()
        {
            SetViewBagData();
            return View();
        }
        public ActionResult CreateWorkspace(WorkSpace workSpace)
        {
            var userId = User.Identity.GetUserId<int>();

            var ToDoList = new ToDoList();

            var role = new UserWorkspaceRoles
            {
                CanManageUsers = true,
                CanManageBudget = true,
                CanManageProjects = true,
                CanManageRoles = true,
                CanManageSettings = true,
                CanManageStatistic = true
            };



            _context.ToDoLists.Add(ToDoList);
            _context.UserWorkspaceRoles.Add(role);
            _context.SaveChanges();

            workSpace.ApplicationUserId = userId;
            workSpace.Created = DateTime.Now;
            workSpace.IsDefault = false;
            workSpace.ToDoListId = ToDoList.Id;
            workSpace.CurrencyId = 1;

            var userWorkspace = new UserWorkspace
            {
                Invited = DateTime.Now,
                Joined = DateTime.Now,
                ApplicationUserId = userId,
                WorkspaceId = workSpace.Id,
                StatusId = Status.Accepted,
                UserWorkspaceRolesId = role.Id,

            };

            _context.WorkSpaces.Add(workSpace);
            _context.UserWorkspace.Add(userWorkspace);

            _context.SaveChanges();

            return RedirectToAction("Index", "Workspace");
        }

        [HttpPost]
        public void SwitchWorkspace(int id)
        {
            var userId = User.Identity.GetUserId<int>();

            var workspace = _context.UserSettingses.SingleOrDefault(u => u.ApplicationUserId == userId);

            if (workspace == null)
                return;

            workspace.currentWorkspace = id;

            _context.SaveChanges();

            WorkSpaceSingleton.Instance.Id = id;

        }
        /// <summary>
        /// Showing workspace by ID clicked
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult WorkspaceDetail(int id)
        {

            var userId = User.Identity.GetUserId<int>();
            var roleId = _context.UserWorkspace.SingleOrDefault(c => c.ApplicationUserId == userId && c.WorkspaceId == id).UserWorkspaceRolesId;

            var workspace = _context.WorkSpaces.SingleOrDefault(c => c.Id == id);
            var userWorkspace = _context.UserWorkspace.Where(c => c.WorkspaceId == id && c.StatusId == Status.Accepted).Include(c => c.ApplicationUser).ToList();

            var viewModel = new WorkspaceDetailViewModel
            {
                WorkSpace = workspace,
                UserWorkspaces = userWorkspace,
                Role = _context.UserWorkspaceRoles.SingleOrDefault(c => c.Id == roleId),
                UserId = userId

            };
            SetViewBagData();

            return View(viewModel);
        }

        [HttpPost]
        public void InviteUser(int userId, WorkSpace workspace)
        {

            //Find if UserInvitation already exist
            var isUserInDatabase = _context.UserWorkspace.Include(c => c.Status)
                                           .SingleOrDefault(c => c.ApplicationUserId == userId && c.WorkspaceId == workspace.Id);


            //Check if user exist and was in workspace
            if (isUserInDatabase != null && isUserInDatabase.StatusId == Status.LeftWorkspace)
            {
                isUserInDatabase.StatusId = Status.Pending;
                _context.SaveChanges();
            }
            else if (isUserInDatabase == null)
            {
                //Creating new invitation if user does not exist
                var userWorkspace = new UserWorkspace
                {
                    Invited = DateTime.Now,
                    Joined = DateTime.Now,
                    ApplicationUserId = userId,
                    WorkspaceId = workspace.Id,
                    StatusId = Status.Pending
                };

                _context.UserWorkspace.Add(userWorkspace);
                _context.SaveChanges();
            }
        }

        [HttpPost]
        public void InvitationHandler(int usWorkspaceId, bool acceptInvitation)
        {

            var userWorkspace = _context.UserWorkspace.SingleOrDefault(c => c.Id == usWorkspaceId);



            if (acceptInvitation)
            {

                var role = new UserWorkspaceRoles
                {
                    CanManageUsers = false,
                    CanManageBudget = false,
                    CanManageProjects = false,
                    CanManageRoles = false,
                    CanManageSettings = false,
                    CanManageStatistic = false
                };

                _context.UserWorkspaceRoles.Add(role);
                _context.SaveChanges();

                userWorkspace.Joined = DateTime.Now;
                userWorkspace.StatusId = Status.Accepted;
                userWorkspace.UserWorkspaceRolesId = role.Id;
            }
            else
            {
                _context.UserWorkspace.Remove(userWorkspace);
            }

            _context.SaveChanges();
        }
        [HttpGet]
        public PartialViewResult GetWorkspacePartialViewResult(int invitationId)
        {

            int workspaceId = (int)_context.UserWorkspace.SingleOrDefault(c => c.Id == invitationId).WorkspaceId;

            var viewModel = new WorkspaceViewModel
            {
                WorkSpace = _context.WorkSpaces.SingleOrDefault(c => c.Id == workspaceId),
                currentWorkspaceId = WorkSpaceSingleton.Instance.Id
            };

            return PartialView("Components/_WorkspaceEntry", viewModel);
        }



        /*Handle body content navigation*/

        /// <summary>
        /// Return partial view with all users in current workspace
        /// </summary>
        [HttpGet]
        public PartialViewResult GetUsersPartialView(int workspaceId)
        {
            var userId = User.Identity.GetUserId<int>();
            var roleId = _context.UserWorkspace.SingleOrDefault(c => c.ApplicationUserId == userId && c.WorkspaceId == workspaceId).UserWorkspaceRolesId;

            var workspace = _context.WorkSpaces.SingleOrDefault(c => c.Id == workspaceId);
            var userWorkspace = _context.UserWorkspace.Where(c => c.WorkspaceId == workspaceId && c.StatusId == Status.Accepted).Include(c => c.ApplicationUser).ToList();

            var viewModel = new WorkspaceDetailViewModel
            {
                WorkSpace = workspace,
                UserWorkspaces = userWorkspace,
                Role = _context.UserWorkspaceRoles.SingleOrDefault(c => c.Id == roleId)
            };

            return PartialView("Components/Users/_UserSummaryPartialView", viewModel);
        }
        /// <summary>
        /// Return partial view for invite new user
        /// </summary>
        [HttpGet]
        public PartialViewResult GetInvitePartialView(int workspaceId)
        {
            var workspace = _context.WorkSpaces.SingleOrDefault(c => c.Id == workspaceId);
            var userWorkspace = _context.UserWorkspace.Where(c => c.WorkspaceId == workspaceId).Include(c => c.ApplicationUser).ToList();

            var viewModel = new WorkspaceDetailViewModel
            {
                WorkSpace = workspace,
                UserWorkspaces = userWorkspace,

            };

            return PartialView("Components/Users/_InviteUserPartialView", viewModel);
        }
        /// <summary>
        /// Return partial view for all projects in workspace
        /// </summary>
        [HttpGet]
        public PartialViewResult GetProjectsPartialView(int workspaceId)
        {
            var projects = _context.Projects.Where(c => c.WorkSpaceId == workspaceId && c.StatusId == Status.Active)
                                            .Include(c => c.ApplicationUser)
                                            .Include(c => c.Status)
                                            .Include(c => c.Client).ToList();

            var projectTimeSpent = new List<ProjectTotalTime>();

            foreach (var pr in projects)
            {

                var prTimeEntries = _context.TimeEntries.Where(c => c.ProjectId == pr.Id).ToList();

                var prTime = prTimeEntries.Any() ? prTimeEntries.Sum(c => c.SpendedTime) / 60 / 60 : 0;

                var entry = new ProjectTotalTime
                {
                    Project = pr,
                    ProjectTime = prTime
                };

                projectTimeSpent.Add(entry);
            }

            var viewModel = new WorkspaceDetailViewModel
            {
                Projects = _context.Projects.Where(c => c.WorkSpaceId == workspaceId && c.StatusId == Status.Active)
                                            .Include(c => c.ApplicationUser)
                                            .Include(c => c.Status)
                                            .Include(c => c.Client),
                ProjectsTotalTime = projectTimeSpent,


            };

            return PartialView("Components/Project/_ProjectsPartialView", viewModel);
        }
        /// <summary>
        /// User permissions partil view
        /// </summary>
        [HttpGet]
        public PartialViewResult GetUserPermissionsPartialView(int workspaceId)
        {


            var viewModel = new WorkspaceUserPermissions
            {

                UserWorkspaces = _context.UserWorkspace.Where(c => c.WorkspaceId == workspaceId && c.StatusId == Status.Accepted)
                                                       .Include(c => c.ApplicationUser)
                                                       .Include(c => c.UserWorkspaceRoles),

            };

            return PartialView("Components/Roles/_UserRolesPartialView", viewModel);
        }
        /// <summary>
        /// Save new permissions for users
        /// </summary>
        /// <param name="permissions"></param>
        /// <param name="userId"></param>
        /// <param name="workspaceId"></param>
        [HttpPost]
        public void SavePermissions(UserWorkspaceRoles permissions, int userId, int workspaceId)
        {

            var permId = _context.UserWorkspace.SingleOrDefault(c => c.ApplicationUserId == userId && c.WorkspaceId == workspaceId).UserWorkspaceRolesId;
            //Find current permissions
            var curPermissions = _context.UserWorkspaceRoles.SingleOrDefault(c => c.Id == permId);

            Mapper.Map(permissions, curPermissions);
            //Save contex
            _context.SaveChanges();
        }
        [HttpGet]
        public PartialViewResult UserDetailPartialView(int userId, int workspaceId)
        {

            var completeWorkspaceBillings = _context.Billingses.Include(c => c.Currency)
                                                    .Where(c => c.WorkSpaceId == workspaceId && c.Status.Id == Status.Active);


            var assignedBillingsUserWorkspace = _context.UserWorkspaceBillings.Where(c => c.ApplicationUserId == userId && c.WorkSpaceId == workspaceId && c.StatusId == Status.Active);

            var assignedBillings = completeWorkspaceBillings.Where(c => assignedBillingsUserWorkspace.Any(x => x.BillingsId == c.Id) && c.StatusId == Status.Active);
            var availableBillings = completeWorkspaceBillings.Where(c => !assignedBillingsUserWorkspace.Any(x => x.BillingsId == c.Id) && c.StatusId == Status.Active);

            var viewModel = new UserDetailWorkspaceViewModel
            {
                UserWorkspace = _context.UserWorkspace.Include(c => c.ApplicationUser).SingleOrDefault(c => c.WorkspaceId == workspaceId && c.ApplicationUserId == userId),
                AssignedBillings = assignedBillings,
                AvailableBillings = availableBillings,
            };

            return PartialView("Components/Users/_UserDetailPartialView", viewModel);
        }
        [HttpPost]
        public void AddBillingToUser(int billingId, int userId, int workspaceId)
        {
            var userBilling = new UserWorkspaceBilling
            {
                ApplicationUserId = userId,
                BillingsId = billingId,
                WorkSpaceId = workspaceId,
                StatusId = Status.Active
            };

            _context.UserWorkspaceBillings.Add(userBilling);
            _context.SaveChanges();
        }
        [HttpPost]
        public void RemoveBillingFromUser(int billingId, int userId, int workspaceId)
        {
            var userBilling = _context.UserWorkspaceBillings.FirstOrDefault(c => c.ApplicationUserId == userId && c.BillingsId == billingId && c.WorkSpaceId == workspaceId);

            if (userBilling == null)
                return;

            _context.UserWorkspaceBillings.Remove(userBilling);
            _context.SaveChanges();
        }

        [HttpPost]
        public JsonResult RemoveUserFromWorkspace(int workspaceId, int userId, bool userWasKicked)
        {
            var userWorkspace = _context.UserWorkspace.SingleOrDefault(c => c.WorkspaceId == workspaceId && c.ApplicationUserId == userId);

            if (userWorkspace == null)
                return new JsonResult();

            var userRoles = _context.UserWorkspaceRoles.SingleOrDefault(c => c.Id == userWorkspace.UserWorkspaceRolesId);

            userWorkspace.StatusId = Status.LeftWorkspace;

            _context.SaveChanges();



            CheckIfUserNeedChangeWorkspace(workspaceId, userId);

            if (userRoles == null)
                return new JsonResult();

            userRoles.CanManageUsers = false;
            userRoles.CanManageBudget = false;
            userRoles.CanManageProjects = false;
            userRoles.CanManageRoles = false;
            userRoles.CanManageSettings = false;
            userRoles.CanManageStatistic = false;

            _context.SaveChanges();

            return Json(userWasKicked ? new { result = "Redirect", url = Url.Action("WorkspaceDetail", "Workspace", new { id = workspaceId }) } : new { result = "Redirect", url = Url.Action("Index", "Workspace") });
        }

        /// <summary>
        /// Check if user was fired from selected workspace, if was, then change it
        /// </summary>
        /// <param name="workspaceId"></param>
        /// <param name="userId"></param>
        private void CheckIfUserNeedChangeWorkspace(int workspaceId, int userId)
        {
            var selectedWorkspaceId = _context.UserSettingses.Single(c => c.ApplicationUserId == userId).currentWorkspace;

            if (selectedWorkspaceId != workspaceId)
                return;

            var userSettings = _context.UserSettingses.Single(c => c.ApplicationUserId == userId);

            var defaultWorkspaceId = _context.WorkSpaces.Single(c => c.ApplicationUserId == userId && c.IsDefault).Id;

            userSettings.currentWorkspace = defaultWorkspaceId;

            WorkSpaceSingleton.Instance.Id = defaultWorkspaceId;

            _context.SaveChanges();
        }
    }
}
