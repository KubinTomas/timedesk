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
    public class TagController : Controller
    {
        private ApplicationDbContext _context;

        public TagController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Tag
        public ActionResult Index()
        {
            var workSpaceId = WorkSpaceSingleton.Instance.Id;

            SetViewBagData();

            var viewModel = new TagViewModel
            {
                Tags = _context.Tags.Where(p => p.WorkSpaceId == workSpaceId),
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
        public void DeleteTag(int id)
        {

            var tag = _context.Tags.First(c => c.Id == id);

            //Delete entries relative to deleted tag
            var usedTags = _context.TimeEntryTags.Where(c => c.TagId == id);

            _context.TimeEntryTags.RemoveRange(usedTags);
            _context.Tags.Remove(tag);

            _context.SaveChanges();

        }
        [HttpPost]
        public void AddTag(Tag tag)
        {

            var newTag = new Tag
            {
                Name = tag.Name,
                WorkSpaceId = WorkSpaceSingleton.Instance.Id,
        };
           

            _context.Tags.Add(newTag);
            _context.SaveChanges();

        }
        [HttpGet]
        public PartialViewResult GetTagPartialViewResult()
        {
            var workSpaceId = WorkSpaceSingleton.Instance.Id;

            var viewModel = new TagViewModel
            {
                Tags = _context.Tags.Where(p => p.WorkSpaceId == workSpaceId),
            };
            return PartialView("_TagsPartialView", viewModel);
        }
    }
}