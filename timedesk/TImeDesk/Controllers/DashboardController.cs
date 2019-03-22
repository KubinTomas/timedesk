using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using Microsoft.AspNet.Identity;
using TImeDesk.Models;
using TImeDesk.Models.Convertors;
using TImeDesk.Models.Database;
using TImeDesk.Models.Others;
using TImeDesk.Models.Singletons;
using TImeDesk.ViewModel;

namespace TImeDesk.Controllers
{
    public class DashboardController : Controller
    {

        private ApplicationDbContext _context;

        public DashboardController()
        {
            _context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        // GET: Dashboard
        public ActionResult Index()
        {
            SetViewBagData();

            //  exChangeTest();
            return View();
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
        [HttpGet]
        public JsonResult GetTimeEntriesPieChart(DateTime startDate, DateTime endDate)
        {
            var workSpaceId = WorkSpaceSingleton.Instance.Id;
            var userId = User.Identity.GetUserId<int>();

            endDate = getEndDate(endDate);


            var Projects = _context.Projects.Where(p => p.WorkSpaceId == workSpaceId).ToList();

            var result = _context.TimeEntries.Where(p => p.ApplicationUserId == userId &&  p.WorkspaceId == workSpaceId && p.StartedDate >= startDate && p.StartedDate < endDate && p.IsFinished).GroupBy(o => o.ProjectId)
                .Select(g => new { ProjectId = g.Key, total = g.Sum(i => i.SpendedTime) });

            var entries = new List<CustomTimeEntry>();

            string prName;
            string color;
          


            foreach (var group in result)
            {



                var pr = Projects.SingleOrDefault(c => c.Id == group.ProjectId);

                if (pr != null)
                {
                    prName = pr.Name;
                    color = pr.ColorString;

                } else
                {
                    prName = "No Project";
                    color = "#D7D7D7";
                }

                if(color == null)
                    color = "#D7D7D7";



                entries.Add(new CustomTimeEntry { ProjectName = prName, TotalTimeSpend = group.total, Color = color });


            }

            return Json(new { JSONList = entries }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetTimeEntriesColumnChart(DateTime startDate, DateTime endDate)
        {
            var workSpaceId = WorkSpaceSingleton.Instance.Id;
            var userId = User.Identity.GetUserId<int>();

            //            var entries = _context.TimeEntries.Where(c => c.ApplicationUserId == userId && c.WorkspaceId == workSpaceId).ToList();

            var Projects = _context.Projects.Where(p => p.WorkSpaceId == workSpaceId).ToList();
           
            endDate = endDate.AddHours(23);
            endDate = endDate.AddMinutes(59);
            endDate = endDate.AddSeconds(59);
            endDate = endDate.AddMilliseconds(59);

            var result = _context.TimeEntries.Where(p => p.ApplicationUserId == userId && p.WorkspaceId == workSpaceId && p.StartedDate >= startDate && p.StartedDate < endDate &&p.IsFinished).GroupBy(o => EntityFunctions.TruncateTime(o.StartedDate))
                .Select(g => new { StartedDate = g.Key, total = g.Sum(i => i.SpendedTime) });

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
        [HttpGet]
        public JsonResult GetDashboardSummary(DateTime startDate, DateTime endDate)
        {
            // User properties for select data
            var workSpaceId = WorkSpaceSingleton.Instance.Id;
            var userId = User.Identity.GetUserId<int>();

            //Set end date time
            endDate = getEndDate(endDate);

            // Select all projects
            var Projects = _context.Projects.Where(p => p.WorkSpaceId == workSpaceId).ToList();

            //Select projects and spent time in interval
            var result = _context.TimeEntries.Where(p => p.ApplicationUserId == userId && p.WorkspaceId == workSpaceId && p.StartedDate >= startDate && p.StartedDate < endDate && p.IsFinished)
            .GroupBy(o => o.ProjectId)
            .Select(g => new { ProjectId = g.Key, total = g.Sum(i => i.SpendedTime) }).ToList();

            //Properties for saving project count, time and most tracket project
            int lastSpentTime = 0;

            int projectCount = 0;
            int totalTimeSpent = 0;
            string mostTracketProject = "No project";


            var entries = new List<DashboardSummary>();
            //Finding most tracket project and counting time
            foreach (var group in result)
            {
                var pr = Projects.SingleOrDefault(c => c.Id == group.ProjectId);

                if (pr != null && group.total > lastSpentTime)
                {
                    lastSpentTime = group.total;
                    mostTracketProject = pr.Name;
                }
             

                projectCount++;

                totalTimeSpent += group.total; 
            }
            // Select all Billings and Currencis in DB for our User
         //   var Billings = _context.Billingses.Where(c => c.Id == workSpaceId).ToList();
            var Currencies = _context.Currency.ToList();
      
            //Select all time entries in time period
            var res = _context.TimeEntries.Where(p => p.ApplicationUserId == userId && p.WorkspaceId == workSpaceId && p.StartedDate >= startDate && p.StartedDate < endDate && p.IsFinished).ToList();

            int curId = _context.UserSettingses.First(c => c.ApplicationUserId == userId).CurrencyId;
            string parseTo = _context.Currency.First(c => c.Id == curId).Name;

            string symbol = Currencies.FirstOrDefault(c => c.Name == parseTo).Symbol ?? "Error";

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
                    string parseFrom = Currencies.First(b => b.Id == Billing.CurrencyId).Name;
                    totalEarned += CurrencyConvertor.ConvertCurrency(earned, parseFrom, parseTo);
                }
                    
            }

           entries.Add(new DashboardSummary { ProjectCount = projectCount, mostTracket = mostTracketProject, TotalTime = totalTimeSpent, Symbol = symbol, TotalEarned = (int)totalEarned });

            return Json(new { JSONList = entries }, JsonRequestBehavior.AllowGet);
        }

        private DateTime getEndDate(DateTime endDate)
        {
            endDate = endDate.AddHours(23);
            endDate = endDate.AddMinutes(59);
            endDate = endDate.AddSeconds(59);
            endDate = endDate.AddMilliseconds(59);

            return endDate;
        }
    }
}