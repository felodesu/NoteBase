using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using NoteBase.Models;
using Microsoft.AspNetCore.Authorization;

namespace NoteBase.Controllers
{
    [AllowAnonymous]
    public class BaseController : Controller
    {
        private DbConnection dbConnection;

        private UserManager<Users> userManager;
		private SignInManager<Users> signInManager;

        public BaseController(DbModel dbModel, UserManager<Users> userMng, SignInManager<Users> signInMng)
        {
            dbConnection = new DbConnection(dbModel);
            userManager = userMng;
			signInManager = signInMng;
        }

        [HttpGet]       
        public ViewResult Authenticate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate(AuthenticationModel authenticationResponse)
        {
            if (ModelState.IsValid)
            {
				Users user = await userManager.FindByNameAsync(authenticationResponse.Login);
				if (user != null)
				{
					await signInManager.SignOutAsync();
					Microsoft.AspNetCore.Identity.SignInResult res = await signInManager.PasswordSignInAsync(authenticationResponse.Login, authenticationResponse.Password, false, false);

					if (res.Succeeded)
						return RedirectToAction("MyNotes", "NoteLibrary", new { area = "" });

					else ModelState.AddModelError(nameof(AuthenticationModel.Password), "Password is incorrect");
				}
				else
				{
					ModelState.AddModelError(nameof(AuthenticationModel.Login), "User does not exist");
				}
               
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
				if (await GetUser(registration.Name) == null)
				{
					IdentityResult result = await userManager.CreateAsync(new Users { UserName = registration.Name }, registration.Password);
					if (result.Succeeded)
					{
						return RedirectToAction("Authenticate");

					}
                }
                else
                {
					ModelState.AddModelError(nameof(RegistrationModel.Name), "User already exists");
					return View();
				}
            }

            return View();
        }

        public async Task<Users> GetUser(string Name)
        {
			Users user = await userManager.FindByNameAsync(Name);

			return user;
        }
    }
}