using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using TImeDesk.Models;
using TImeDesk.Models.Database;
using TImeDesk.Models.Singletons;
using TImeDesk.ViewModel;

namespace TImeDesk.Controllers
{
    public class BillingController : Controller
    {

        private ApplicationDbContext _context;

        public BillingController()
        {
            _context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        // GET: Billing
        public ActionResult Index()
        {
            var workSpaceId = WorkSpaceSingleton.Instance.Id;
            var userId = User.Identity.GetUserId<int>();

            SetViewBagData();

            var viewModel = new BillingCurrencyViewModel
            {
                Currencies = _context.Currency.ToList(),
                Billingses = _context.Billingses.Where(c => c.WorkSpaceId == workSpaceId && c.StatusId == Status.Active)
                                                .Include(c => c.Status)
                                                .Include(c => c.Currency)
                                                .ToList()
            };

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
        public void AddBilling(Billings bill)
        {
            var workSpaceId = WorkSpaceSingleton.Instance.Id;
            var userId = User.Identity.GetUserId<int>();

            var billing = new Billings
            {
                Name = bill.Name,
                HourRate = bill.HourRate.Replace(",", "."),
                WorkSpaceId = workSpaceId,
                CurrencyId = bill.CurrencyId,
                StatusId = Status.Active
            };

            _context.Billingses.Add(billing);
            _context.SaveChanges();

            var userBilling = new UserWorkspaceBilling
            {
                ApplicationUserId = userId,
                WorkSpaceId = workSpaceId,
                BillingsId = billing.Id,
                StatusId = Status.Active
            };

            _context.UserWorkspaceBillings.Add(userBilling);
            _context.SaveChanges();
        }
        [HttpGet]
        public PartialViewResult GetBillingsPartialViewResult()
        {
            var workSpaceId = WorkSpaceSingleton.Instance.Id;
            var userId = User.Identity.GetUserId<int>();

            var viewModel = new BillingCurrencyViewModel
            {
                Currencies = _context.Currency.ToList(),
                Billingses = _context.Billingses.Where(c => c.WorkSpaceId == workSpaceId && c.StatusId == Status.Active)
                    .Include(c => c.Status)
                    .Include(c => c.Currency)
                    .ToList()
            };
            return PartialView("_BillingsPartialView", viewModel);
        }
        [HttpPost]
        public void DeleteBilling(int id)
        {

            var billing = _context.Billingses.First(c => c.Id == id);


            billing.StatusId = Status.Deleted;

            //Save changes
            _context.SaveChanges();
        }

    }
}