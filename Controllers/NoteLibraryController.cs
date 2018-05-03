using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NoteBase.Models;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace NoteBase.Controllers
{
    [Authorize]
    public class NoteLibraryController : Controller
    {
        private DbConnection dbConnection;

        public NoteLibraryController(DbModel dbModel)
        {
            dbConnection = new DbConnection(dbModel);
        }

        [HttpGet]
        public async Task<IActionResult> MyNotes()
        {
            List<SharedNotes> userNotes = await dbConnection.GetUserNotes(User.Identity.Name);
            userNotes = SharedNotes.Distinct(userNotes);

            var users = await dbConnection.GetUsers();
            users.RemoveAll(u => u.Name == User.Identity.Name);
       
            ViewBag.Users = users;

            return View("MyNotes", new UsersNotesModel { sharedNotes = userNotes});
        }

        [HttpPost]
        public async Task<IActionResult> CreateNote(UsersNotesModel note)
        {
            if (ModelState.IsValid)
            {
                await dbConnection.CreateNote(new Note { Header = note.Header, Content = note.Content, Timestamp = note.Timestamp.ToBinary() }, User.Identity.Name);
            }
            return RedirectToAction("MyNotes");
        }

        [HttpPost]
        public async Task<IActionResult> ShareNote(UsersNotesModel usersNotes)
        {
            await dbConnection.ShareNote((int)usersNotes.Note_Id, (int)usersNotes.User_Id, User.Identity.Name);
            return RedirectToAction("MyNotes");
        }

        [HttpGet]
        public async Task<IActionResult> ViewSharedNotes()
        {
            ViewBag.SharedNotes = await dbConnection.GetSharedNotes(User.Identity.Name);
            return View("SharedNotes");
        }

        public JsonResult CheckDateTimeValid(DateTime Timestamp)
        {
            if (Timestamp > DateTime.Now)
                return Json(data: true);

            return Json(data: false);
        }
    }
}