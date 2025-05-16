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

namespace App_Agenda_Fatec.Controllers
{

    public class RoleController : Controller
    {

        private readonly MongoDBContext _context;

        private readonly RoleManager<AppRole> _app_roles_manager;

        public RoleController(RoleManager<AppRole> app_roles_manager)
        {

            this._context = new MongoDBContext();

            this._app_roles_manager = app_roles_manager;

        }

        // GET: Role
        public async Task<IActionResult> Index()
        {

            List<Role> roles = new List<Role>();

            foreach (AppRole app_role in (await this._context.Roles.Find(FilterDefinition<AppRole>.Empty).ToListAsync()))
            {

                roles.Add(this.GenerateEquivalentObject(app_role));

            }

            return View(roles);

        }

        // GET: Role/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {

            if (id == null)
            {

                return NotFound();

            }

            var role = await this._context.Roles.Find(r => r.Id == id).FirstOrDefaultAsync();

            if (role == null)
            {

                return NotFound();

            }

            ViewBag.Activation = (role.Active ?? false) ? "Ativado" : "Desativado";

            return View(this.GenerateEquivalentObject(role));

        }

        // GET: Role/Create
        public IActionResult Create()
        {

            return View(new Role());

        }

        // POST: Role/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Active")] Role role)
        {

            if (ModelState.IsValid)
            {

                AppRole app_role = new AppRole();

                app_role.Id = Guid.NewGuid();

                app_role.Name = role.Name;

                app_role.Active = role.Active;

                IdentityResult create_result = await this._app_roles_manager.CreateAsync(app_role);

                if (create_result.Succeeded)
                {

                    return RedirectToAction(nameof(Index));

                }

                else
                {

                    foreach (IdentityError error in create_result.Errors)
                    {

                        ModelState.AddModelError("", error.Description);

                    }

                }

            }

            return View(role);

        }

        // GET: Role/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {

            if (id == null)
            {

                return NotFound();

            }

            var role = await this._context.Roles.Find(r => r.Id == id).FirstOrDefaultAsync();

            if (role == null)
            {

                return NotFound();

            }

            return View(this.GenerateEquivalentObject(role));

        }

        // POST: Role/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Active")] Role role)
        {

            if (id != role.Id)
            {

                return NotFound();

            }

            if (ModelState.IsValid)
            {

                try
                {

                    AppRole app_role = await this._context.Roles.Find(r => r.Id == id).FirstOrDefaultAsync();

                    if (app_role == null)
                    {

                        return NotFound();

                    }

                    app_role.Name = role.Name;

                    app_role.Active = role.Active;

                    IdentityResult update_result = await this._app_roles_manager.UpdateAsync(app_role);

                    if (update_result.Succeeded)
                    {

                        return RedirectToAction(nameof(Index));

                    }

                    else
                    {

                        foreach (IdentityError error in update_result.Errors)
                        {

                            ModelState.AddModelError("", error.Description);

                        }

                    }

                }

                catch (DbUpdateConcurrencyException)
                {

                    if (!RoleExists(role.Id))
                    {

                        return NotFound();

                    }

                    else
                    {

                        throw;

                    }

                }

            }

            return View(role);

        }

        // GET: Role/Delete/5
        public async Task<IActionResult> ModifyActivation(Guid? id)
        {

            if (id == null)
            {

                return NotFound();

            }

            var role = await this._context.Roles.Find(r => r.Id == id).FirstOrDefaultAsync();

            if (role == null)
            {

                return NotFound();

            }

            ViewBag.Activation = (role.Active ?? false) ? "Ativado" : "Desativado";

            return View(this.GenerateEquivalentObject(role));

        }

        // POST: Role/Delete/5
        [HttpPost, ActionName("ModifyActivation")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ModifyActivationConfirmed(Guid id)
        {

            var role = await _context.Roles.Find(r => r.Id == id).FirstOrDefaultAsync();

            if (role != null)
            {

                role.Active = !role.Active;

                IdentityResult soft_delete_result = await this._app_roles_manager.UpdateAsync(role);

                if (!soft_delete_result.Succeeded)
                {

                    foreach (IdentityError error in soft_delete_result.Errors)
                    {

                        ModelState.AddModelError("", error.Description);

                    }

                    return View(this.GenerateEquivalentObject(role));

                }

            }

            return RedirectToAction(nameof(Index));

        }

        private bool RoleExists(Guid id)
        {

            return this._context.Roles.Find(r => r.Id == id).Any();

        }

        private Role GenerateEquivalentObject(AppRole document)
        {

            Role role = new Role()
            {

                Id = document.Id,

                Name = document.Name,

                Active = document.Active

            };

            return role;

        }

    }

}