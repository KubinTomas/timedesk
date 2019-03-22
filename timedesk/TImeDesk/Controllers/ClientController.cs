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
    public class ClientController : Controller
    {


        private ApplicationDbContext _context;

        public ClientController()
        {
                _context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Client
        public ActionResult Index()
        {

            var workSpaceId = WorkSpaceSingleton.Instance.Id;
            SetViewBagData();

            var viewModel = new ClientViewModel
            {
                Clients = _context.Clients.Where(c => c.WorkSpaceId == workSpaceId && c.StatusId == Status.Active)
                                          .Include(c => c.WorkSpace)
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
        [HttpPost]
        public void AddClient(Client cl)
        {
            var workSpaceId = WorkSpaceSingleton.Instance.Id;
            var userId = User.Identity.GetUserId<int>();

            var client = new Client
            {
                Name = cl.Name,
                WorkSpaceId =  workSpaceId,
                ApplicationUserId = userId,
                StatusId = Status.Active
            };

            _context.Clients.Add(client);
            _context.SaveChanges();

        }
        [HttpPost]
        public void DeleteClient(int id)
        {
            
            var client = _context.Clients.First(c => c.Id == id);

        
            client.StatusId = Status.Deleted;

            //Save changes
            _context.SaveChanges();
        }

        [HttpGet]
        public PartialViewResult GetClientPartialViewResult()
        {
            var workSpaceId = WorkSpaceSingleton.Instance.Id;

            var viewModel = new ClientViewModel
            {
                Clients = _context.Clients.Where(c => c.WorkSpaceId == workSpaceId && c.StatusId == Status.Active)
                    .Include(c => c.WorkSpace)
                    .ToList()
            };
            return PartialView("_ClientsPartialView", viewModel);
        }
    }
}