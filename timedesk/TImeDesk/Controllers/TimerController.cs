using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using TImeDesk.Models;
using TImeDesk.Models.Database;
using TImeDesk.Models.Singletons;
using TImeDesk.ViewModel;

namespace TImeDesk.Controllers
{
    [Authorize]
    public class TimerController : Controller
    {
        private readonly ApplicationDbContext _context;
        // How much days should be loaded
        private readonly int _days;



        public TimerController()
        {
            _context = new ApplicationDbContext();
            //Loading time entris old 7 days * times clicked on load more
            _days = 7;

            

        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Timer
        public ActionResult Index()
        {


            SetViewBagData();
            //Default value
            WorkSpaceSingleton.Instance.TimeEntryLoadCount = 1;

          var viewModel = GetTimeEntryViewModel();

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
        public void StartTimer(TimeEntryViewModel viewModel, bool saveTimeEntry, List<int> tagsId)
        {

            var workSpaceId = WorkSpaceSingleton.Instance.Id;
            var userId = User.Identity.GetUserId<int>();


            // Select running timeEtry for User in actual Workspace
            var timeEntryinDb = _context.TimeEntries.Where(t => t.WorkspaceId == workSpaceId && t.ApplicationUserId == userId)
                .FirstOrDefault(t => !t.IsFinished);

          
            var timeEntry = Mapper.Map<TimeEntryViewModel, TimeEntry>(viewModel);

            if (timeEntryinDb == null)
            {
                timeEntry.WorkspaceId = workSpaceId;
                timeEntry.ApplicationUserId = userId;



                // Add tags to time entry
                if (tagsId != null && tagsId.Any())
                {
                    for (int i = 0; i < tagsId.Count; i++)
                    {
                        var entryTag = new TimeEntryTag
                        {
                            TimeEntryId = timeEntry.Id,
                            TagId = tagsId.ElementAt(i)
                        };

                        _context.TimeEntryTags.Add(entryTag);

                    }
                }

                _context.TimeEntries.Add(timeEntry);
                _context.SaveChanges();
            }
            else
            {
                Mapper.Map(timeEntry, timeEntryinDb);

                if (saveTimeEntry)
                {
                    timeEntryinDb.IsFinished = true;
                    timeEntryinDb.SpendedTime =  (int)(timeEntryinDb.EndedDate - timeEntryinDb.StartedDate).TotalSeconds;  // In Seconds
                }

                _context.SaveChanges();
            }


        }
        [HttpPost]
        public void AddTimeEntry(TimeEntryViewModel viewModel)
        {
            var workSpaceId = WorkSpaceSingleton.Instance.Id;
            var userId = User.Identity.GetUserId<int>();

            var timeEntry = Mapper.Map<TimeEntryViewModel, TimeEntry>(viewModel);

            var startedDate = timeEntry.StartedDate;
            var endedDate = timeEntry.EndedDate;

            timeEntry.WorkspaceId = workSpaceId;
            timeEntry.ApplicationUserId = userId;
            timeEntry.SpendedTime = (int)(timeEntry.EndedDate - timeEntry.StartedDate).TotalSeconds;

            _context.TimeEntries.Add(timeEntry);
            _context.SaveChanges();
        }
        [HttpDelete]
        public void DeleteRunningEntry()
        {
            var workSpaceId = WorkSpaceSingleton.Instance.Id;
            var userId = User.Identity.GetUserId<int>();


            // Select running timeEtry for User in actual Workspace
            var timeEntryinDb = _context.TimeEntries.Where(t => t.WorkspaceId == workSpaceId && t.ApplicationUserId == userId)
                .FirstOrDefault(t => !t.IsFinished);

            if(timeEntryinDb == null)
                return;

            _context.TimeEntries.Remove(timeEntryinDb);
            _context.SaveChanges();
        }

        //Get TimeEntryPartialView
        [HttpGet]
        public PartialViewResult GetTimerPartialViewResult()
        {

           var viewModel = GetTimeEntryViewModel();

            return PartialView("_TimeEntryPartialView", viewModel);
        }

        [HttpGet]
        public JsonResult GetTimeEntryJsonModel()
        {
            var viewModel = GetTimeEntryViewModel();

            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetTasksInProjectJsonModel(int prId)
        {
            var tasks = _context.ProjectTasks.Where(c => c.ProjectId == prId && c.StatusId == Status.Active);

            return Json(tasks, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public PartialViewResult GetTimeEntriesPartialViewResult()
        {
            var viewModel = GetTimeEntryViewModel();

            return PartialView("_TimeEntriesPartialView", viewModel);
        }
        [HttpGet]
        public PartialViewResult GetManualControlPartialView()
        {
            return PartialView("Components/_ManualTimerPartialView");
        }
        [HttpGet]
        public PartialViewResult GetAutomaticControlPartialView()
        {
            var viewModel = new TimeEntryViewModel
            {
                IsFinished = true
            };

            return PartialView("Components/_AutomaticTimerPartialView", viewModel);
        }
        [HttpPost]
        public void UpdateTimeEntry(TimeEntry timeEntry, string description)
        {
            var timeEntryInDb = _context.TimeEntries.FirstOrDefault(c => c.Id == timeEntry.Id);

            if (timeEntryInDb != null)
            {
                timeEntryInDb.ProjectId = timeEntry.ProjectId;
                timeEntryInDb.ProjectTaskId = timeEntry.ProjectTaskId;
                timeEntryInDb.Description = timeEntry.Description;
                timeEntryInDb.BillingsId = timeEntry.BillingsId;

                _context.SaveChanges();

            }
            
        }

        [HttpPost]
        public void IncreaseLoadedHistory(int historyValue)
        {
            WorkSpaceSingleton.Instance.TimeEntryLoadCount = historyValue;
        }
        [HttpPost]
        public void DeleteEntry(int id)
        {

            var entry = _context.TimeEntries.First(c => c.Id == id);

            _context.TimeEntries.Remove(entry);

            //Save changes
            _context.SaveChanges();

            
        }

        private TimeEntryViewModel GetTimeEntryViewModel()
        {
            var workSpaceId = WorkSpaceSingleton.Instance.Id;
            var userId = User.Identity.GetUserId<int>();
            var dateOffest = System.DateTime.Today.AddDays((-1) * _days * WorkSpaceSingleton.Instance.TimeEntryLoadCount);

            // Select running timeEtry for User in actual Workspace, maybe without workspaceId to have only 1 running time entry on user
            var timeEntryinDb = _context.TimeEntries.Where(t => t.WorkspaceId == workSpaceId && t.ApplicationUserId == userId)
                .FirstOrDefault(t => !t.IsFinished);

            var userProjects = _context.UserProjects
                .Where(p => p.WorkSpaceId == workSpaceId && p.ApplicationUserId == userId)
                .AsEnumerable()
                .Select(r => (int)r.ProjectId);

            var completeWorkspaceBillings = _context.Billingses.Include(c => c.Currency)
                .Where(c => c.WorkSpaceId == workSpaceId && c.Status.Id == Status.Active);


            var assignedBillingsUserWorkspace = _context.UserWorkspaceBillings.Where(c => c.ApplicationUserId == userId && c.WorkSpaceId == workSpaceId && c.StatusId == Status.Active);

            var assignedBillings = completeWorkspaceBillings.Where(c => assignedBillingsUserWorkspace.Any(x => x.BillingsId == c.Id) && c.StatusId == Status.Active).ToList();

            var viewModel = new TimeEntryViewModel
            {
                Projects = _context.Projects.Where(p => userProjects.Contains(p.Id) && p.StatusId == Status.Active).ToList(),
                TimeEntries = _context.TimeEntries.Where(t => t.WorkspaceId == workSpaceId && t.ApplicationUserId == userId && t.IsFinished).Include(c => c.Project)
                                                  .OrderByDescending(t => t.StartedDate).Where(c=>c.StartedDate > dateOffest).ToList(),
                ProjectTasks = Enumerable.Empty<ProjectTask>(),
                WorkspaceId = workSpaceId,
                Description = null,
                ProjectId = null,
                ProjectTaskId = null,
                IsFinished = true,
                Project = null,
                ProjectTask = null,   
                AllProjectTasks = _context.ProjectTasks.ToList(),
                AllSelectedTags = _context.TimeEntryTags.Include(c => c.Tag).ToList(),
                AllBillingses = _context.Billingses.Where(c => c.WorkSpaceId == workSpaceId).ToList(),
                Billingses = _context.Billingses.Where(c => c.WorkSpaceId == workSpaceId && c.StatusId == Status.Active).ToList(),
                AssignedBillingses = assignedBillings,
                Currency = _context.Currency.ToList(),            
                AllTags = _context.Tags.Where(c => c.WorkSpaceId == workSpaceId).ToList(),
                SelectedTags = Enumerable.Empty<TimeEntryTag>(),
                AutomaticTimer = _context.UserSettingses.First(c => c.ApplicationUserId == userId).AutomaticTimer


            };

            if (timeEntryinDb != null)
            {

                ChangeTimerStatus();

                var selectedTags = _context.TimeEntryTags.Where(c => c.TimeEntryId == timeEntryinDb.Id).Include(c => c.Tag).ToList();

                viewModel = new TimeEntryViewModel
                {
                    Projects = _context.Projects.Where(p => userProjects.Contains(p.Id) && p.StatusId == Status.Active).ToList(),
                    TimeEntries = _context.TimeEntries.Where(t => t.WorkspaceId == workSpaceId && t.ApplicationUserId == userId && t.IsFinished).Include(c=> c.Project).OrderByDescending(t => t.StartedDate).ToList(),
                    ProjectTasks = _context.ProjectTasks.Where(c => c.ProjectId == timeEntryinDb.ProjectId).ToList(),
                    WorkspaceId = workSpaceId,
                    Id = timeEntryinDb.Id,
                    StartedDate = timeEntryinDb.StartedDate,
                    Description = timeEntryinDb.Description,
                    ProjectId = timeEntryinDb.ProjectId,
                    ProjectTaskId = timeEntryinDb.ProjectTaskId,
                    BillingsId = timeEntryinDb.BillingsId,
                    IsFinished = timeEntryinDb.IsFinished,
                    Project = _context.Projects.SingleOrDefault(c => c.Id == timeEntryinDb.ProjectId),
                    ProjectTask = _context.ProjectTasks.SingleOrDefault(c => c.Id == timeEntryinDb.ProjectTaskId),
                    AllProjectTasks = _context.ProjectTasks.ToList(),
                    AllSelectedTags = _context.TimeEntryTags.Include(c=>c.Tag).ToList(),
                    AllBillingses = _context.Billingses.Where(c => c.WorkSpaceId == workSpaceId).ToList(),
                    SelectedTags = selectedTags,
                    AllTags = _context.Tags.Where(c => c.WorkSpaceId == workSpaceId).ToList(),
                    Billingses = _context.Billingses.Where(c => c.WorkSpaceId == workSpaceId && c.StatusId == Status.Active).ToList(),
                    AssignedBillingses = assignedBillings,
                    Currency = _context.Currency.ToList(),
                    AutomaticTimer = true

                };
            }

            return viewModel;
        }

        private void ChangeTimerStatus()
        {
            var userId = User.Identity.GetUserId<int>();

            var settings = _context.UserSettingses.First(c => c.ApplicationUserId == userId);

            settings.AutomaticTimer = true;

            _context.SaveChanges();
        }
        [HttpPost]
        public void AddTagToTimeEntry(int tagId, int entryId)
        {
            var entryTag = new TimeEntryTag
            {
                TagId = tagId,
                TimeEntryId = entryId
            };

            _context.TimeEntryTags.Add(entryTag);
            _context.SaveChanges();
        }
        [HttpPost]
        public void RemoveTagFromTimeEntry(int tagId, int entryId)
        {
            var entryTag = _context.TimeEntryTags.FirstOrDefault(c => c.TagId == tagId && c.TimeEntryId == entryId);

            if(entryTag == null)
                return;

            _context.TimeEntryTags.Remove(entryTag);
            _context.SaveChanges();
        }

    }
}