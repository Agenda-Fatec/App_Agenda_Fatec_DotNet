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

        private readonly SignInManager<AppUser> _login_manager;

        private readonly UserManager<AppUser> _app_users_manager;

        public AuthController(SignInManager<AppUser> login_manager, UserManager<AppUser> app_users_manager)
        {

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

    }

}