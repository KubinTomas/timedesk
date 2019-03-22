using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TImeDesk.Models.Others
{
    public class GlobalViewBagUserPermission : ActionFilterAttribute
    {
        private ApplicationDbContext _context;

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {

            //using (_context = new ApplicationDbContext())
            //{
            //    var currentUserId = HttpContext.Current.User.Identity.GetUserId<int>();

            //    // User is signed
            //    if (currentUserId != 0)
            //    {

            //        var currentWorkspaceId = _context.UserSettingses.Single(c => c.ApplicationUserId == currentUserId).currentWorkspace;
            //        var permissionId = _context.UserWorkspace.Single(c => c.ApplicationUserId == currentUserId && c.WorkspaceId == currentWorkspaceId).UserWorkspaceRolesId;

            //        filterContext.Controller.ViewBag.UserWorkspaceRoles = _context.UserWorkspaceRoles.Single(c => c.Id == permissionId);
            //    }

            //    _context.Dispose();

            //}

        }
    }
}