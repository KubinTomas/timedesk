using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using TImeDesk.Models;
using TImeDesk.ViewModel;

namespace TImeDesk.Controllers
{
    public class ProfileController : Controller
    {
        private ApplicationDbContext _context;

        public ProfileController()
        {
            _context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        // GET: Profile
        [Authorize]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId<int>();

            var settings = _context.UserSettingses.First(c => c.ApplicationUserId == userId);

            var viewModel = new ProfilCurrencyViewModel
            {
                Currencies = _context.Currency.ToList(),
                CurrencyId = settings.CurrencyId,
                UserId = userId
                
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
        public void ChangeCurrency(int CurrencyId)
        {
            var userId = User.Identity.GetUserId<int>();

            var settings = _context.UserSettingses.First(c => c.ApplicationUserId == userId);

            settings.CurrencyId = CurrencyId;

            _context.SaveChanges();
        }

        [HttpPut]
        public void ChangeTimerStatus()
        {
            var userId = User.Identity.GetUserId<int>();

            var settings = _context.UserSettingses.First(c => c.ApplicationUserId == userId);

            settings.AutomaticTimer = !settings.AutomaticTimer;

            _context.SaveChanges();
        }
    }
}