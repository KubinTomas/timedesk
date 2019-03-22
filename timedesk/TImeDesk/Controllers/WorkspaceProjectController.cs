using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using TImeDesk.Models;
using TImeDesk.Models.Convertors;
using TImeDesk.Models.Database;
using TImeDesk.Models.Others;
using TImeDesk.ViewModel;
using TImeDesk.ViewModel.Workspace.Project;

namespace TImeDesk.Controllers
{
    // Controller for all project operations
    public class WorkspaceProjectController : Controller
    {

        private ApplicationDbContext _context;


        public WorkspaceProjectController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        /// <summary>
        /// Return partial view with detail of project
        /// </summary>
        [HttpGet]
        public PartialViewResult GetProjectDetailPartialView(int projectId, int workspaceId)
        {


            var userWorkspace = _context.UserWorkspace.Where(c => c.WorkspaceId == workspaceId)
                .Include(c => c.ApplicationUser);

            var project = _context.Projects.SingleOrDefault(c => c.Id == projectId);


            var viewModel = new ProjectWorkspaceDetailViewModel
            {
                Project = _context.Projects.SingleOrDefault(c => c.Id == projectId),

            };

            return PartialView("_ProjectDetailPartialView", viewModel);
        }

        [HttpGet]
        public PartialViewResult GetProjectStatisticPartialView(int projectId, int workspaceId)
        {


            var project = _context.Projects.Include(c => c.Client).Include(c=> c.ApplicationUser).SingleOrDefault(c => c.Id == projectId);

            // Select all Billings and Currencis in DB for our User
            var billingses = _context.Billingses.Where(c => c.WorkSpaceId == workspaceId).ToList();
            var currencies = _context.Currency.ToList();

            //Select all time entries in time period
            var allTimeEntries = _context.TimeEntries.Where(p => p.WorkspaceId == workspaceId && p.ProjectId == projectId && p.IsFinished).ToList();

            string parseTo = _context.WorkSpaces.Include(c=> c.Currency).Single(c=>c.Id == workspaceId).Currency.Name;

            decimal totalEarned = 0;
            int totalTimeSpent = 0;
            //Couting total money earned
            foreach (var entry in allTimeEntries)
            {
                if (entry.BillingsId != null)
                {
                    // Time * Hour * Value
                    var billing = billingses.First(c => c.Id == entry.BillingsId);
                    decimal time = entry.SpendedTime / 60m / 60m;
                    decimal earned = time * decimal.Parse(billing.HourRate.Replace(".", ","));
                    string parseFrom = currencies.First(b => b.Id == billing.CurrencyId).Name;

                    totalEarned += CurrencyConvertor.ConvertCurrency(earned, parseFrom, parseTo);
                }

                totalTimeSpent += entry.SpendedTime;

            }

            var viewModel = new ProjectWorkspaceDetailViewModel
            {
                Project = project,
                TotalAssignedUsers = _context.UserProjects.Count(c => c.WorkSpaceId == workspaceId && c.ProjectId == projectId),
                WorkspaceCurrencyName = _context.WorkSpaces.Include(c=>c.Currency).Single(c=> c.Id == workspaceId).Currency.Name,
                TotalWorkedTime = totalTimeSpent / 60 /60 ,
                Money = totalEarned.ToString("0.##"),
                DeadlineProgressBarValue = GetDeadlineProgressBarValue(project.Created, project.DeadLine),
                HourLimitProgressBarValue = GetHourLimitProgressBarValue(project.HourLimit, totalTimeSpent/60/60),
                BudgetProgressBarValue = GetBudgetProgressBarValue(project.Budget, totalEarned)



                //UserProjects = _context.UserProjects.Where(c => c.ProjectId == projectId)
                //     .Include(c => c.ApplicationUser),
                //   WorkspaceUsers = userWorkspace

            };

            return PartialView("_ProjectDetailStatisticPartialView", viewModel);
        }
        /// <summary>
        /// Return % for progress bar
        /// </summary>
        /// <param name="startDay"></param>
        /// <param name="endDay"></param>
        /// <returns></returns>
        private int GetDeadlineProgressBarValue(DateTime startDay, DateTime? endDay)
        {

            startDay = new DateTime(startDay.Year, startDay.Month,startDay.Day,0,0,0);
            

            if (!endDay.HasValue || (endDay.Value.Date <= DateTime.Today))
                return 100;

            var totalDaysToFinish = (endDay.Value - startDay).TotalDays;
            var daysLeft = (endDay.Value - DateTime.Today).TotalDays;

            if(daysLeft <= 0)
                return 100;

            return (int)(((totalDaysToFinish - daysLeft) / totalDaysToFinish)*100);

        }

        private int GetHourLimitProgressBarValue(int? totalHours, int spentHours)
        {
            if(!totalHours.HasValue || spentHours >= totalHours)
                return 100;

            decimal totalHoursDob = (decimal)totalHours.Value;
            decimal spentHoursDob = (decimal)spentHours;

         

            return (int)((spentHoursDob / totalHoursDob) * 100);
        }

        private int GetBudgetProgressBarValue(float? budget, decimal spentMoney)
        {
            if (!budget.HasValue)
                return 100;

            decimal budgetDecimal = (decimal) budget.Value;

            if (spentMoney >= budgetDecimal)
                return 100;

          

            return (int)((spentMoney / budgetDecimal) * 100);
        }

        /// <summary>
        /// Return all users working on project
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="workspaceId"></param>
        /// <returns></returns>
        [HttpGet]
        public PartialViewResult GetUsersProjectPartialView(int projectId, int workspaceId)
        {
            var userWorkspace = _context.UserWorkspace.Where(c => c.WorkspaceId == workspaceId)
                .Include(c => c.ApplicationUser);

            var userProject = _context.UserProjects.Where(c => c.ProjectId == projectId)
                    .Include(c => c.ApplicationUser).ToList();


            var userProjectTimeSpent = new List<UserProjectSpentTime>();

            foreach (var user in userProject)
            {

                var prTimeEntries = _context.TimeEntries.Where(c => c.ProjectId == projectId && c.ApplicationUserId == user.ApplicationUserId).ToList();

                var prTime = prTimeEntries.Any() ? prTimeEntries.Sum(c => c.SpendedTime) / 60 / 60 : 0;

                var entry = new UserProjectSpentTime
                {
                    User = _context.Users.Single(c=>c.Id == user.ApplicationUserId),
                    TimeWorkedOnProject = prTime
                };

                userProjectTimeSpent.Add(entry);
            }


            var viewModel = new ProjectWorkspaceDetailViewModel
            {
                Project = _context.Projects.SingleOrDefault(c => c.Id == projectId),
                UserProjects = userProject,
                UserProjectTimeList = userProjectTimeSpent


            };

            return PartialView("_UsersProjectPartialView", viewModel);
        }
        /// <summary>
        /// Return all users available to assign on project
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="workspaceId"></param>
        /// <returns></returns>
        [HttpGet]
        public PartialViewResult GetAddUsersToProjectPartialView(int projectId, int workspaceId)
        {

            var userWorkspace = _context.UserWorkspace.Where(c => c.WorkspaceId == workspaceId)
                .Include(c => c.ApplicationUser);

            var projectUsers = _context.UserProjects.Where(c => c.ProjectId == projectId)
                .Include(c => c.ApplicationUser);

            var usersToAssing = userWorkspace.Where(c => !projectUsers.Any(x => x.ApplicationUserId == c.ApplicationUserId));

            var viewModel = new ProjectWorkspaceDetailViewModel
            {
                Project = _context.Projects.SingleOrDefault(c => c.Id == projectId),
                UsersToAssign = usersToAssing

            };

            return PartialView("_UsersToAddOnProjectPartialView", viewModel);

        }

        [HttpPost]
        public void AddUserToProject(int projectId, int workspaceId, int userId)
        {

            var userProject = _context.UserProjects.FirstOrDefault(c =>
                c.ProjectId == projectId && c.WorkSpaceId == workspaceId && c.ApplicationUserId == userId);

            if (userProject != null)
                return;

            userProject = new UserProject
            {
                ApplicationUserId = userId,
                WorkSpaceId = workspaceId,
                ProjectId = projectId,
            };

            _context.UserProjects.Add(userProject);
            _context.SaveChanges();
        }
        [HttpPost]
        public void RemoveUserFromProject(int projectId, int workspaceId, int userId)
        {

            var userProject = _context.UserProjects.FirstOrDefault(c =>
                c.ProjectId == projectId && c.WorkSpaceId == workspaceId && c.ApplicationUserId == userId);

            if (userProject == null)
                return;

            _context.UserProjects.Remove(userProject);
            _context.SaveChanges();
        }

        [HttpPost]
        public void SaveProject(Project project, int workspaceId)
        {

            var userId = User.Identity.GetUserId<int>();

            if (project.Id == 0)
            {
                project.Created = DateTime.Now;
                project.StatusId = Status.Active;
                project.ApplicationUserId = userId;
                project.WorkSpaceId = workspaceId;
                project.CurrencyId = _context.WorkSpaces.Single(c => c.Id == workspaceId).CurrencyId;

                _context.Projects.Add(project);
                _context.SaveChanges();

                var userProject = new UserProject
                {
                    ApplicationUserId = userId,
                    WorkSpaceId = workspaceId,
                    ProjectId = project.Id,
                };

                _context.UserProjects.Add(userProject);

            }

            var projectInDb = _context.Projects.SingleOrDefault(c => c.Id == project.Id);

            Mapper.Map(project, projectInDb);

            _context.SaveChanges();


        }

        [HttpGet]
        public PartialViewResult GetCreateProjectViewResult(int workspaceId, int projectId)
        {

            var project = _context.Projects.Include(c => c.Client).SingleOrDefault(c => c.Id == projectId) ?? new Project();

            var viewModel = new CreateWorkspaceProjectPartialView
            {
                WorkspaceId = workspaceId,
                Project = project,
                CurrencyName = _context.WorkSpaces.Include(c => c.Currency).Single(c => c.Id == workspaceId).Currency.Name,
                Clients = _context.Clients.Where(c => c.WorkSpaceId == workspaceId && c.StatusId == Status.Active),

            };

            return PartialView("_CreateProjectPartialView", viewModel);
        }

        [HttpGet]
        public PartialViewResult GetProjectTasksPartialView(int workspaceId, int projectId)
        {

            var project = _context.Projects.Single(c => c.Id == projectId);
            var tasks = _context.ProjectTasks.Where(c => c.ProjectId == projectId);

            List<TaskTimeSpent> taskTimeSpentList = new List<TaskTimeSpent>();


            foreach (var task in tasks)
            {

                var taskTimeEntries = _context.TimeEntries.Where(c => c.ProjectTaskId == task.Id && c.IsFinished);

                var totalTimeSpent = taskTimeEntries.Any() ? taskTimeEntries.Sum(c => c.SpendedTime) / 60 / 60 : 0;

                var taskTime = new TaskTimeSpent
                {
                    Task = task,
                    TotalTimeSpent = totalTimeSpent
                };

                taskTimeSpentList.Add(taskTime);
            }


            var viewModel = new ProjectTasksViewModel
            {
                Project = project,
                Tasks = tasks,
                WorkspaceId = workspaceId,
                TasksTimeSpent = taskTimeSpentList,
            };

            return PartialView("_ProjectTasksPartialView", viewModel);
        }
    }
}