using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using TImeDesk.Models;
using TImeDesk.Models.Database;

namespace TImeDesk.Controllers
{
    public class WorkspaceNavbarsController : Controller
    {

        private ApplicationDbContext _context;

        public WorkspaceNavbarsController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        [HttpGet]
        public PartialViewResult GetDashboardNavbarPartialViewResult(int workspaceId)
        {
            var userId = User.Identity.GetUserId<int>();
            var roleId = _context.UserWorkspace.SingleOrDefault(c => c.ApplicationUserId == userId && c.WorkspaceId == workspaceId).UserWorkspaceRolesId;
            var Role = _context.UserWorkspaceRoles.SingleOrDefault(c => c.Id == roleId);


            return PartialView("_wWorkspaceNavigationMenu", Role);
        }
        [HttpGet]
        public PartialViewResult GetProjectNavbarPartialViewResult()
        {
            return PartialView("_wProjectNavigationMenu");
        }

       
    }
}