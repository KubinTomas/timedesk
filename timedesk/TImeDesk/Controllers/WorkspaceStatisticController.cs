using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using TImeDesk.Models;
using TImeDesk.Models.Convertors;
using TImeDesk.Models.Database;
using TImeDesk.Models.Others;
using TImeDesk.Models.Singletons;
using TImeDesk.ViewModel.Workspace.Statistic;

namespace TImeDesk.Controllers
{
    public class WorkspaceStatisticController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WorkspaceStatisticController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        [HttpGet]
        public PartialViewResult GetWorkspaceStatistic(int workspaceId)
        {

            var workspaceUser = _context.UserWorkspace.Where(c => c.WorkspaceId == workspaceId && c.StatusId == Status.Accepted);



            var viewModel = new WorkspaceStatisticViewModel
            {
                WorkspaceUsers = _context.Users.Where(c => workspaceUser.Any(x => c.Id == x.ApplicationUserId)),
                Projects = _context.Projects.Where(c => c.WorkSpaceId == workspaceId && c.StatusId == Status.Active)
        };

            return PartialView("_WorkspaceStatisticPartialView", viewModel);
        }
        [HttpGet]
        public JsonResult GetWorkspaceSummary(DateTime startDate, DateTime endDate, int userId, int projectId, int workspaceId)
        {


          
            //Set end date time
            endDate = GetEndDate(endDate);

            // Select all projects
            var allWorkspaceProjects = _context.Projects.Where(p => p.WorkSpaceId == workspaceId).ToList();

            //Select projects and spent time in interval
            var result = GetFilteredTimeEntries(startDate,endDate,userId, projectId, workspaceId).GroupBy(o => o.ProjectId)
                .Select(g => new { ProjectId = g.Key, total = g.Sum(i => i.SpendedTime) }).ToList();

            //result = _context.TimeEntries.Where(p => p.WorkspaceId == workspaceId && p.StartedDate >= startDate && p.StartedDate < endDate && p.IsFinished).GroupBy(o => o.ProjectId)
            //    .Select(g => new { ProjectId = g.Key, total = g.Sum(i => i.SpendedTime) });;

            //Properties for saving project count, time and most tracket project
            int lastSpentTime = 0;

            int projectCount = 0;
            int totalTimeSpent = 0;
            string mostTracketProject = "No project";


            var entries = new List<DashboardSummary>();
            //Finding most tracket project and counting time
            foreach (var group in result)
            {
                var pr = allWorkspaceProjects.SingleOrDefault(c => c.Id == group.ProjectId);

                if (pr != null && group.total > lastSpentTime)
                {
                    lastSpentTime = group.total;
                    mostTracketProject = pr.Name;
                }


                projectCount++;

                totalTimeSpent += group.total;
            }


            // Select all Billings and Currencies
            var billings = _context.Billingses.Where(c => c.Id == workspaceId).ToList();
            var currencies = _context.Currency.ToList();

            //Select all time entries in time period
            var res = GetFilteredTimeEntries(startDate, endDate, userId, projectId, workspaceId).ToList();

            int curId = _context.WorkSpaces.First(c => c.Id == workspaceId).CurrencyId.Value;
            string parseTo = _context.Currency.First(c => c.Id == curId).Name;

            string symbol = currencies.FirstOrDefault(c => c.Name == parseTo).Symbol ?? "Error";

            decimal totalEarned = 0;
            //Couting total money earned
            foreach (var e in res)
            {
                if (e.BillingsId != null)
                {
                    // Time * Hour * Value
                    var Billing = _context.Billingses.First(c => c.Id == e.BillingsId);
                    decimal time = e.SpendedTime / 60m / 60m;
                    decimal earned = time * decimal.Parse(Billing.HourRate.Replace(".", ","));
                    string parseFrom = currencies.First(b => b.Id == Billing.CurrencyId).Name;
                    totalEarned += CurrencyConvertor.ConvertCurrency(earned, parseFrom, parseTo);
                }

            }

            entries.Add(new DashboardSummary { ProjectCount = projectCount, mostTracket = mostTracketProject, TotalTime = totalTimeSpent, Symbol = symbol, TotalEarned = (int)totalEarned });

            return Json(new { JSONList = entries }, JsonRequestBehavior.AllowGet);
        }

        private IEnumerable<TimeEntry> GetFilteredTimeEntries(DateTime startDate, DateTime endDate, int userId, int projectId, int workspaceId)
        {

            var res = _context.TimeEntries.Where(p => p.WorkspaceId == workspaceId && p.StartedDate >= startDate && p.StartedDate < endDate && p.IsFinished);

            if (userId != 0)
                res = res.Where(c => c.ApplicationUserId == userId);

            if (projectId != 0)
                res = res.Where(c => c.ProjectId == projectId);

            return res;

        }

        [HttpGet]
        public JsonResult GetTimeEntriesPieChart(DateTime startDate, DateTime endDate, int userId, int projectId, int workspaceId)
        {


            endDate = GetEndDate(endDate);


            var workspaceProjects = _context.Projects.Where(p => p.WorkSpaceId == workspaceId).ToList();

            var result = GetFilteredTimeEntries(startDate, endDate, userId, projectId, workspaceId).GroupBy(o => o.ProjectId)
                .Select(g => new { ProjectId = g.Key, total = g.Sum(i => i.SpendedTime) });

            var entries = new List<CustomTimeEntry>();

            string prName;
            string color;



            foreach (var group in result)
            {
                var pr = workspaceProjects.SingleOrDefault(c => c.Id == group.ProjectId);

                if (pr != null)
                {
                    prName = pr.Name;
                    color = pr.ColorString;
                }
                else
                {
                    prName = "No Project";
                    color = "#D7D7D7";
                }

                if (color == null)
                    color = "#D7D7D7";



                entries.Add(new CustomTimeEntry { ProjectName = prName, TotalTimeSpend = group.total, Color = color });


            }

            return Json(new { JSONList = entries }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetTimeEntriesProjectPieChart(DateTime startDate, DateTime endDate, int userId, int projectId, int workspaceId)
        {


            endDate = GetEndDate(endDate);


            var workspaceTaks = _context.ProjectTasks.Where(p => p.WorkSpaceId == workspaceId).ToList();

            var result = GetFilteredTimeEntries(startDate, endDate, userId, projectId, workspaceId).GroupBy(o => o.ProjectTaskId)
                .Select(g => new { TaskId = g.Key, total = g.Sum(i => i.SpendedTime) });

            var entries = new List<CustomTimeEntry>();

            string prName;
            string color;



            foreach (var group in result)
            {
                var task = workspaceTaks.SingleOrDefault(c => c.Id == group.TaskId);

                if (task != null)
                {
                    prName = task.Name;
                    color = task.ColorString;
                }
                else
                {
                    prName = "No Task";
                    color = "#D7D7D7";
                }

                if (color == null)
                    color = "#D7D7D7";



                entries.Add(new CustomTimeEntry { ProjectName = prName, TotalTimeSpend = group.total, Color = color });


            }

            return Json(new { JSONList = entries }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetTimeEntriesColumnChart(DateTime startDate, DateTime endDate, int userId, int projectId, int workspaceId)
        {

            endDate = GetEndDate(endDate);

            dynamic result;

            if (userId == 0 && projectId == 0)
            {
                 result = _context.TimeEntries.Where(p => p.WorkspaceId == workspaceId && p.StartedDate >= startDate && p.StartedDate < endDate && p.IsFinished)
                    .GroupBy(o => EntityFunctions.TruncateTime(o.StartedDate))
                    .Select(g => new { StartedDate = g.Key, total = g.Sum(i => i.SpendedTime) });
            }else if (projectId == 0 && userId != 0)
            {
                 result = _context.TimeEntries.Where(p => p.WorkspaceId == workspaceId && p.StartedDate >= startDate && p.StartedDate < endDate && p.IsFinished && p.ApplicationUserId == userId)
                    .GroupBy(o => EntityFunctions.TruncateTime(o.StartedDate))
                    .Select(g => new { StartedDate = g.Key, total = g.Sum(i => i.SpendedTime) });
            }else if (userId == 0 && projectId != 0)
            {
                 result = _context.TimeEntries.Where(p => p.WorkspaceId == workspaceId && p.StartedDate >= startDate && p.StartedDate < endDate && p.IsFinished && p.ProjectId == projectId)
                    .GroupBy(o => EntityFunctions.TruncateTime(o.StartedDate))
                    .Select(g => new { StartedDate = g.Key, total = g.Sum(i => i.SpendedTime) });
            }
            else
            {
                 result = _context.TimeEntries.Where(p => p.WorkspaceId == workspaceId && p.StartedDate >= startDate && p.StartedDate < endDate && p.IsFinished && p.ProjectId == projectId && p.ApplicationUserId == userId)
                    .GroupBy(o => EntityFunctions.TruncateTime(o.StartedDate))
                    .Select(g => new { StartedDate = g.Key, total = g.Sum(i => i.SpendedTime) });
            }
            
            
           

            //var result = GetFilteredTimeEntries(startDate, endDate, userId, projectId, workspaceId).GroupBy(o => EntityFunctions.TruncateTime(o.StartedDate))
            //    .Select(g => new { StartedDate = g.Key, total = g.Sum(i => i.SpendedTime) });

            var entries = new List<CustomTimeEntry>();


            DateTime curDate = new DateTime();



            foreach (var group in result)
            {

                if (group.StartedDate != null)
                    curDate = (DateTime)group.StartedDate;


                entries.Add(new CustomTimeEntry { StartedDate = curDate.Date.ToString("dd/MM/yyyy"), TotalTimeSpend = group.total });


            }

            return Json(new { JSONList = entries }, JsonRequestBehavior.AllowGet);
        }
        private DateTime GetEndDate(DateTime endDate)
        {
            endDate = endDate.AddHours(23);
            endDate = endDate.AddMinutes(59);
            endDate = endDate.AddSeconds(59);
            endDate = endDate.AddMilliseconds(59);

            return endDate;
        }
    }
}