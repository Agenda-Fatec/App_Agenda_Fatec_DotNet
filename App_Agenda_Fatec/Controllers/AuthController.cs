using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using App_Agenda_Fatec.Data;
using App_Agenda_Fatec.Models;
using MongoDB.Driver;

using Microsoft.AspNetCore.Identity;

using System.ComponentModel.DataAnnotations;

namespace App_Agenda_Fatec.Controllers
{

    [Route("Account")]
    public class AuthController : Controller
    {

        private readonly MongoDBContext _context;

        private readonly SignInManager<AppUser> _login_manager;

        private readonly UserManager<AppUser> _app_users_manager;

        public AuthController(SignInManager<AppUser> login_manager, UserManager<AppUser> app_users_manager)
        {

            this._context = new MongoDBContext();

            this._login_manager = login_manager;

            this._app_users_manager = app_users_manager;

        }

        // GET: Account/Login
        [HttpGet("Login")]
        public IActionResult Login()
        {

            return View();

        }

        // POST: Account/Login
        [HttpPost("Login")]
        public async Task<IActionResult> Login([Required][EmailAddress(ErrorMessage = "E-mail inválido.")] string email, [Required] string password, bool keep_connected)
        {

            if (ModelState.IsValid)
            {

                AppUser? app_user = await this._app_users_manager.FindByEmailAsync(email);

                if (app_user != null)
                {

                    Microsoft.AspNetCore.Identity.SignInResult login_result = await this._login_manager.PasswordSignInAsync(app_user, password, keep_connected, false);

                    if (login_result.Succeeded)
                    {

                        CookieOptions login_cookie_settings = new CookieOptions()
                        {

                            HttpOnly = true,

                            Expires = DateTimeOffset.UtcNow.AddDays(7)

                        };

                        Response.Cookies.Append(".Login.User", app_user.Id.ToString(), login_cookie_settings);

                        return RedirectToAction("Index", "Home");

                    }

                }

                ModelState.AddModelError(nameof(email), "Erro ao efetuar o login! Revise seus dados.");

            }

            return View();

        }

        // GET: Account/Logout
        [HttpGet("Logout")]
        public async Task<IActionResult> Logout()
        {

            Response.Cookies.Delete(".Login.User");

            await this._login_manager.SignOutAsync();

            return RedirectToAction("Index", "Home");

        }

        // GET: Account/AccessDenied
        [HttpGet("AccessDenied")]
        public IActionResult AccessDenied()
        {

            return View();

        }

        // GET: Auth/Profile
        [HttpGet("Profile")]
        public async Task<IActionResult> Profile()
        {

            User? login_user = await UserController.GenerateEquivalentObject(await this._app_users_manager.FindByIdAsync(Request.Cookies[".Login.User"]));

            if (login_user == null)
            {

                return NotFound();

            }

            List<Scheduling> schedulings = await this._context.Schedulings.Find(s => s.Requestor_Guid == Guid.Parse(Request.Cookies[".Login.User"])).ToListAsync();

            foreach (Scheduling scheduling in schedulings)
            {

                scheduling.Room = await this._context.Rooms.Find(r => r.Id == scheduling.Room_Guid).FirstOrDefaultAsync();

                scheduling.Requestor = await UserController.GenerateEquivalentObject(await this._context.Users.Find(r => r.Id == scheduling.Requestor_Guid).FirstOrDefaultAsync());

                scheduling.Approver = await UserController.GenerateEquivalentObject(await this._context.Users.Find(a => a.Id == scheduling.Approver_Guid).FirstOrDefaultAsync());

            }

            ViewBag.Schedulings = schedulings;

            return View(login_user);

        }

    }

}