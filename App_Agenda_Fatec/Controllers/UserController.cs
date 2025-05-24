using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using App_Agenda_Fatec.Data;
using App_Agenda_Fatec.Models;
using MongoDB.Driver;

using Microsoft.AspNetCore.Identity;

using System.ComponentModel.DataAnnotations;

using System.Text.RegularExpressions;

using Microsoft.AspNetCore.Authorization;

namespace App_Agenda_Fatec.Controllers
{

    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {

        private readonly MongoDBContext _context;

        private readonly UserManager<AppUser> _app_users_manager;

        public UserController(UserManager<AppUser> app_users_manager)
        {

            this._context = new MongoDBContext();

            this._app_users_manager = app_users_manager;

        }

        // GET: User
        public async Task<IActionResult> Index()
        {

            List<User> users = new List<User>();

            foreach (AppUser app_user in (await this._context.Users.Find(u => u.Id != Guid.Parse(Request.Cookies[".Login.User"])).ToListAsync()))
            {

                users.Add(await GenerateEquivalentObject(app_user));

            }

            return View(users);

        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {

            if (id == null)
            {

                return NotFound();

            }

            var user = await _context.Users.Find(u => u.Id == id).FirstOrDefaultAsync();

            if (user == null)
            {

                return NotFound();

            }

            return View(await GenerateEquivalentObject(user));

        }

        // GET: User/Create
        [AllowAnonymous]
        public IActionResult Create()
        {

            return View(new User());

        }

        // POST: User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,Phone,Password,Administrator,Active")] User user, [Required] [Display(Name = "Confirmação de Senha")] string confirmacao_senha)
        {

            if (ModelState.IsValid)
            {

                if (user.Password == confirmacao_senha)
                {

                    AppUser app_user = new AppUser();

                    app_user.Id = Guid.NewGuid();

                    app_user.UserName = Regex.Replace(Models.User.Remove_Accents(user.Name), @"[^a-zA-Z0-9]", "");

                    app_user.Name = user.Name;

                    app_user.Email = user.Email;

                    app_user.PhoneNumber = user.Phone;

                    app_user.Administrator = user.Administrator;

                    app_user.Active = user.Active;

                    IdentityResult user_register_result = await this._app_users_manager.CreateAsync(app_user, user.Password);

                    if (user_register_result.Succeeded)
                    {

                        await this._app_users_manager.AddToRoleAsync(app_user, "Comum");

                        return RedirectToAction(nameof(Index));

                    }

                    foreach (IdentityError error in user_register_result.Errors)
                    {

                        ModelState.AddModelError("", error.Description);

                    }

                }

                else
                {

                    ModelState.AddModelError("", "As senhas passadas não são iguais!");

                }

            }

            return View(user);

        }

        // GET: User/ModifyActivation/5
        public async Task<IActionResult> ModifyActivation(Guid? id)
        {

            if (id == null)
            {

                return NotFound();

            }

            var user = await _context.Users.Find(u => u.Id == id).FirstOrDefaultAsync();

            if (user == null)
            {

                return NotFound();

            }

            return View(await GenerateEquivalentObject(user));

        }

        // POST: User/ModifyActivation/5
        [HttpPost, ActionName("ModifyActivation")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ModifyActivationConfirmed(Guid id)
        {

            var user = await _context.Users.Find(u => u.Id == id).FirstOrDefaultAsync();

            if (user != null)
            {

                user.Active = !user.Active;

                await this._context.Users.ReplaceOneAsync(u => u.Id == id, user);

            }

            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Reclassify(Guid id)
        {

            var user = await this._context.Users.Find(u => u.Id == id).FirstOrDefaultAsync();

            if (user != null)
            {

                user.Administrator = !user.Administrator;

                await this._context.Users.ReplaceOneAsync(u => u.Id == id, user);

                await this._app_users_manager.RemoveFromRoleAsync(user, (user.Administrator) ? "Comum" : "Admin");

                await this._app_users_manager.AddToRoleAsync(user, (user.Administrator) ? "Admin" : "Comum");

            }

            return RedirectToAction(nameof(Index));

        }

        private bool UserExists(Guid id)
        {

            return this._context.Users.Find(u => u.Id == id).Any();

        }

        public static async Task<User> GenerateEquivalentObject(AppUser document)
        {

            MongoDBContext database_connection = new MongoDBContext();

            User user = new User()
            {

                Id = document.Id,

                Name = document.Name,

                Email = document.Email,

                Phone = document.PhoneNumber,

                Administrator = document.Administrator,

                Active = document.Active

            };

            user.Activation_Stats = (user.Active) ? "Ativado" : "Desativado";

            foreach (Guid role_guid in document.Roles)
            {

                AppRole role = await database_connection.Roles.Find(r => r.Id == role_guid).FirstOrDefaultAsync();

                user.Roles = user.Roles.Append(role.Name).ToArray();

            }

            return user;

        }

    }

}