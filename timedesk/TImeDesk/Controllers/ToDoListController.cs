using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TImeDesk.Models;
using TImeDesk.Models.Singletons;
using TImeDesk.ViewModel;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using TImeDesk.Models.Database;

namespace TImeDesk.Controllers
{
    public class ToDoListController : Controller
    {

        private ApplicationDbContext _context;

        // GET: ToDoList
        public ActionResult Index()
        {
            SetViewBagData();

            var userId = User.Identity.GetUserId<int>();

            var workSpaceId = WorkSpaceSingleton.Instance.Id;

            var IsDefault = _context.WorkSpaces.SingleOrDefault(c => c.Id == workSpaceId).IsDefault;

            var ToDoListId = _context.WorkSpaces.FirstOrDefault(c => c.Id == workSpaceId).ToDoListId;

            var privateNotes = _context.ToDoNoteses.Include(c => c.NoteColorTheme).Where(c => c.ToDoListId == ToDoListId && c.StatusId == Status.Private && c.ApplicationUserId == userId ).ToList();

            var publicNotes = _context.ToDoNoteses.Include(c => c.NoteColorTheme).Where(c => c.ToDoListId == ToDoListId && c.StatusId == Status.Public).ToList();

            var viewModel = new NotesViewModel
            {
                PrivateNotes = privateNotes,
                NoteColorThemes = _context.NoteColorThemes.ToList(),
                PublicNotes = publicNotes
            };

            return IsDefault ? View(viewModel) : View("WorkspaceIndex", viewModel);


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

        public ToDoListController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        [HttpGet]
        public PartialViewResult CreateNewNote(bool privateNote)
        {

            var userId = User.Identity.GetUserId<int>();

            var workSpaceId = WorkSpaceSingleton.Instance.Id;

            var ToDoListId = _context.WorkSpaces.FirstOrDefault(c => c.Id == workSpaceId).ToDoListId;
            var ColorThemeId = _context.NoteColorThemes.OrderBy(c => Guid.NewGuid()).Take(1).First().Id;

            var Note = new ToDoNotes
            {
                ToDoListId = ToDoListId,
                NoteColorThemeId = ColorThemeId,
                ApplicationUserId = userId,
                StatusId = privateNote ? Status.Private : Status.Public
        };

            var viewModel = new NotesViewModel
            {
                Note = Note,
                NoteColorThemes = _context.NoteColorThemes
            };

            _context.ToDoNoteses.Add(Note);
            _context.SaveChanges();

            return PartialView("Components/_NotePartialView", viewModel);
        }
        [HttpPost]
        public void DeleteNote(int id)
        {
            var note = _context.ToDoNoteses.FirstOrDefault(c => c.Id == id);

            if (note != null)
            {
                _context.ToDoNoteses.Remove(note);
                _context.SaveChanges();
            }

        }
        [HttpPost]
        public void UpdateNote(int id, string header, string content)
        {
            var Note = _context.ToDoNoteses.SingleOrDefault(c => c.Id == id);

            if (Note == null)
                return;

            Note.Header = header;
            Note.Content = content;

            _context.SaveChanges();
        }
        [HttpGet]
        public JsonResult ChangeNoteColor(int colId, int noteId)
        {
            var Note = _context.ToDoNoteses.SingleOrDefault(c => c.Id == noteId);

            Note.NoteColorThemeId = colId;

            _context.SaveChanges();

            var ColorTheme = _context.NoteColorThemes.FirstOrDefault(c => c.Id == colId);
            
            return Json(ColorTheme, JsonRequestBehavior.AllowGet);
        }
    }
}