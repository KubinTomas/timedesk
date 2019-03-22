using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using TImeDesk.Models;
using TImeDesk.Models.Database;
using TImeDesk.Models.Singletons;
using TImeDesk.ViewModel.Workspace.Settings;

namespace TImeDesk.Controllers
{
    public class WorkspaceSettingsController : Controller
    {

        private ApplicationDbContext _context;

        public WorkspaceSettingsController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }


        [HttpGet]
        public PartialViewResult GetWorkspaceSettíngsPartialViewResult(int workspaceId)
        {

            var viewModel = new WorkspaceSettingsViewModel
            {
                WorkSpace = _context.WorkSpaces.Include(c => c.Currency).Single(c => c.Id == workspaceId),
                TotalUsersInWorkspace = _context.UserWorkspace.Count(c => c.WorkspaceId == workspaceId && c.StatusId == Status.Accepted),
                Currencies = _context.Currency,
            };

            return PartialView("_WorkspaceSettingsPartialView", viewModel);
        }

        [HttpPost]
        public void ChangeCurrency(Currency currency, WorkSpace WorkSpace)
        {
           

            var workspace = _context.WorkSpaces.First(c => c.Id == WorkSpace.Id);

            workspace.CurrencyId = currency.Id;

            _context.SaveChanges();
        }

        [HttpPost]
        public JsonResult DeleteWorkspace(int workspaceId)
        {
            var workspace = _context.UserWorkspace.Where(c => c.WorkspaceId == workspaceId).ToList();
            var userId = User.Identity.GetUserId<int>();

            var singleWorkspace = _context.WorkSpaces.Single(c => c.Id == workspaceId);

          

            foreach (var w in workspace)
            {
                w.StatusId = Status.Deleted;

                if (w.WorkspaceId.HasValue && w.ApplicationUserId.HasValue)
                    CheckIfUserNeedChangeWorkspace(w.WorkspaceId.Value, w.ApplicationUserId.Value);

            }

            _context.SaveChanges();

            return Json(new { result = "Redirect", url = Url.Action("Index", "Workspace") });
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


            if(User.Identity.GetUserId<int>() == userId)
                 WorkSpaceSingleton.Instance.Id = defaultWorkspaceId;

            _context.SaveChanges();
        }
    }
}