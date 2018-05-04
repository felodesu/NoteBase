using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NoteBase.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace NoteBase.Controllers
{
    [AllowAnonymous]
    public class BaseController : Controller
    {
        private DbConnection dbConnection;

        public BaseController(DbModel dbModel)
        {
            dbConnection = new DbConnection(dbModel);
        }

        private async Task Authenticate(string Name)
        {
            var claims = new List<Claim> { new Claim(ClaimsIdentity.DefaultNameClaimType, Name)};
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie");

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        [HttpGet]       
        public ViewResult AuthenticationForm()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AuthenticationForm(AuthenticationModel authenticationResponse)
        {
            if (ModelState.IsValid)
            {
                await Authenticate(authenticationResponse.Login);
                return RedirectToAction("MyNotes", "NoteLibrary", new { area = "" });
            }
            return View();
        }

        [HttpGet]
        public ViewResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationModel registration)
        {
            if (ModelState.IsValid)
            {
                await dbConnection.CreateNewUser(new Users { Name = registration.Name, Password = registration.Password });
                return RedirectToAction("AuthenticationForm");
            }

            return View();
        }

        public JsonResult CheckUserNotExists(string Name)
        {
            if (!dbConnection.CheckUserExists(Name))
            {
                return Json(data: true);
            }

            return Json(data: false);
        }

        public JsonResult CheckUserExists(string Login)
        {
            if (dbConnection.CheckUserExists(Login))
            {
                return Json(data: true);
            }

            return Json(data: false);
        }

        public JsonResult ValidateUser(string Login, string Password)
        {
            if (dbConnection.ValidateUser(new Users { Name = Login, Password = Password }))
            {
                return Json(data: true);
            }

            return Json(data: false);
        }
    }
}