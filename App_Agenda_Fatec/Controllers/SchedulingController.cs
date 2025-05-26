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

using Microsoft.AspNetCore.Authorization;

namespace App_Agenda_Fatec.Controllers
{

    public class SchedulingController : Controller
    {

        private readonly MongoDBContext _context;

        public SchedulingController()
        {

            this._context = new MongoDBContext();

        }

        // GET: Scheduling
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {

            List<Scheduling> schedulings = await this._context.Schedulings.Find(s => s.Approver_Guid == Guid.Parse(Request.Cookies[".Login.User"])).ToListAsync();

            foreach (Scheduling scheduling in schedulings)
            {

                scheduling.Room = await this._context.Rooms.Find(r => r.Id == scheduling.Room_Guid).FirstOrDefaultAsync();

                scheduling.Requestor = await UserController.GenerateEquivalentObject(await this._context.Users.Find(r => r.Id == scheduling.Requestor_Guid).FirstOrDefaultAsync());

                scheduling.Approver = await UserController.GenerateEquivalentObject(await this._context.Users.Find(a => a.Id == scheduling.Approver_Guid).FirstOrDefaultAsync());

            }

            return View(schedulings);

        }

        // GET: Scheduling/Details/5
        [Authorize]
        public async Task<IActionResult> Details(Guid? id)
        {

            if (id == null)
            {

                return NotFound();

            }

            var scheduling = await this._context.Schedulings.Find(s => s.Id == id).FirstOrDefaultAsync();
            
            if (scheduling == null)
            {

                return NotFound();

            }

            scheduling.Room = await this._context.Rooms.Find(r => r.Id == scheduling.Room_Guid).FirstOrDefaultAsync();

            scheduling.Requestor = await UserController.GenerateEquivalentObject(await this._context.Users.Find(r => r.Id == scheduling.Requestor_Guid).FirstOrDefaultAsync());

            scheduling.Approver = await UserController.GenerateEquivalentObject(await this._context.Users.Find(a => a.Id == scheduling.Approver_Guid).FirstOrDefaultAsync());

            return View(scheduling);

        }

        // GET: Scheduling/Create
        [Authorize(Roles = "Comum")]
        public async Task<IActionResult> Create(Guid room_guid)
        {

            List<User> approvers = new List<User>();

            foreach (AppUser app_user in (await this._context.Users.Find(u => u.Administrator).ToListAsync()))
            {

                approvers.Add(await UserController.GenerateEquivalentObject(app_user));

            }

            ViewBag.Approvers = approvers;

            return View(new Scheduling()
            {

                Room_Guid = room_guid,

                Requestor_Guid = Guid.Parse(Request.Cookies[".Login.User"])

            });

        }

        // POST: Scheduling/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Comum")]
        public async Task<IActionResult> Create([Bind("Utilization_Date,Start_Utilization_Time,End_Utilization_Time,Room_Guid,Requestor_Guid,Approver_Guid")] Scheduling scheduling)
        {

            if (ModelState.IsValid)
            {

                scheduling.Id = Guid.NewGuid();

                await this._context.Schedulings.InsertOneAsync(scheduling);

                return RedirectToAction("Index", "Home");

            }

            return View(scheduling);

        }

        // GET: Scheduling/Delete/5
        [Authorize(Roles = "Comum")]
        public async Task<IActionResult> Delete(Guid? id)
        {

            if (id == null)
            {

                return NotFound();

            }

            var scheduling = await this._context.Schedulings.Find(s => s.Id == id).FirstOrDefaultAsync();
            
            if (scheduling == null)
            {

                return NotFound();

            }

            scheduling.Room = await this._context.Rooms.Find(r => r.Id == scheduling.Room_Guid).FirstOrDefaultAsync();

            scheduling.Requestor = await UserController.GenerateEquivalentObject(await this._context.Users.Find(r => r.Id == scheduling.Requestor_Guid).FirstOrDefaultAsync());

            scheduling.Approver = await UserController.GenerateEquivalentObject(await this._context.Users.Find(a => a.Id == scheduling.Approver_Guid).FirstOrDefaultAsync());

            return View(scheduling);

        }

        // POST: Scheduling/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Comum")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {

            if (this.SchedulingExists(id))
            {

                await this._context.Schedulings.DeleteOneAsync(s => s.Id == id);

            }

            return RedirectToAction("Profile", "Auth");

        }

        // GET: Scheduling/Decision
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Decision(Guid id, string situation)
        {

            var scheduling = await this._context.Schedulings.Find(s => s.Id == id).FirstOrDefaultAsync();

            if (scheduling != null)
            {

                scheduling.Situation = situation;

                await this._context.Schedulings.ReplaceOneAsync(s => s.Id == id, scheduling);

            }

            return RedirectToAction(nameof(Index));

        }

        private bool SchedulingExists(Guid id)
        {

            return this._context.Schedulings.Find(s => s.Id == id).Any();

        }

    }

}