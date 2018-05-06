using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using NoteBase.Models;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;

namespace NoteBase.Controllers
{
    [Authorize]
    public class NoteLibraryController : Controller
    {
        private DbConnection dbConnection;
		private UserManager<Users> userManager;

		private Users user;

		public async Task<Users> GetUser(string Name)
		{
			Users user = await userManager.FindByNameAsync(Name);

			return user;
		}

		public NoteLibraryController(DbModel dbModel, UserManager<Users> userMng)
        {
            dbConnection = new DbConnection(dbModel);
			userManager = userMng;
        }

        [HttpGet]
        public async Task<IActionResult> MyNotes()
        {
			user = await userManager.GetUserAsync(HttpContext.User);
            List<SharedNotes> userNotes = await dbConnection.GetUserNotes(user.Id);

			List<Users> users = (from u in userManager.Users
						 where u.Id != user.Id
						 select new Users
						 {
							 UserName = u.UserName,
							 Id = u.Id

						 })
						 .ToList();
       
            ViewBag.Users = users;

            return View("MyNotes", new UserNotesModel { SharedNotes = userNotes});
        }

        [HttpPost]
        public async Task<IActionResult> CreateNote(UserNotesModel note)
        {
			if (ModelState.IsValid)
			{
				user = await userManager.GetUserAsync(HttpContext.User);

				await dbConnection.CreateNote(new Note
				{
					Header = note.Header,
					Content = note.Content,
					Timestamp = note.Timestamp.ToBinary(),
					UserId = user.Id

				});
			}
			return RedirectToAction("MyNotes");
        }

        [HttpPost]
        public async Task<IActionResult> ShareNote(UserNotesModel userNotes)
        {
			user = await userManager.GetUserAsync(HttpContext.User);
			await dbConnection.ShareNote(userNotes.Note_Id, userNotes.User_Id, user.Id);

            return RedirectToAction("MyNotes");
        }

        [HttpGet]
        public async Task<IActionResult> ViewSharedNotes(List<SharedNotes> sharedNotes)
        {
			if (sharedNotes.Count == 0)
			{
				user = await userManager.GetUserAsync(HttpContext.User);
				sharedNotes = await dbConnection.GetSharedNotes(user.Id);
			}

			return View("MySharedNotes", sharedNotes);
        }

        public JsonResult CheckDateTimeValid(DateTime Timestamp)
        {
            if (Timestamp > DateTime.Now)
                return Json(data: true);

            return Json(data: false);
        }
    }
}