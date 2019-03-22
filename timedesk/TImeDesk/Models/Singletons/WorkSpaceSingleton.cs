using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using TImeDesk.Models;
using TImeDesk.Models.Singletons;
using System.Security.Claims;
using TImeDesk.Models.Database;

namespace TImeDesk.Models.Singletons
{
    public sealed class WorkSpaceSingleton
    {
        private static WorkSpaceSingleton _instance = null;
        private static int UserId;

        public int TimeEntryLoadCount { get; set; }
        public int Id { get; set; }
       // public WorkSpace WorkSpace { get; private set; }
        
        public WorkSpaceSingleton()
        {
            var _context = new ApplicationDbContext();

            TimeEntryLoadCount = 1;

            UserId = HttpContext.Current.User.Identity.GetUserId<int>();

            Id = _context.UserSettingses.SingleOrDefault(c => c.ApplicationUserId == UserId).currentWorkspace;

        }
        public static WorkSpaceSingleton Instance => _instance ?? (_instance = new WorkSpaceSingleton());

        public static void RefreshSingeton()
        {
            _instance = null;
        }

        public static void SetWorkspace()
        {
        }
    }
}