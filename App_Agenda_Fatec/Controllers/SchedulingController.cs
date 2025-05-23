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

    public class SchedulingController : Controller
    {

        private readonly MongoDBContext _context;

        private readonly UserManager<AppUser> _app_users_manager;

        public SchedulingController(UserManager<AppUser> app_users_manager)
        {

            this._context = new MongoDBContext();

            this._app_users_manager = app_users_manager;

        }

        // GET: Scheduling
        public async Task<IActionResult> Index()
        {

            List<Scheduling> schedulings = await this._context.Schedulings.Find(FilterDefinition<Scheduling>.Empty).ToListAsync();

            foreach (Scheduling scheduling in schedulings)
            {

                scheduling.Room = await this._context.Rooms.Find(r => r.Id == scheduling.Room_Guid).FirstOrDefaultAsync();

                scheduling.Requestor = await UserController.GenerateEquivalentObject(await this._context.Users.Find(r => r.Id == scheduling.Requestor_Guid).FirstOrDefaultAsync());

                scheduling.Approver = await UserController.GenerateEquivalentObject(await this._context.Users.Find(a => a.Id == scheduling.Approver_Guid).FirstOrDefaultAsync());

            }

            return View(schedulings);

        }

        // GET: Scheduling/Details/5
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
        public async Task<IActionResult> Create([Bind("Id,Request_Date,Request_Time,Utilization_Date,Start_Utilization_Time,End_Utilization_Time,Situation,Room_Guid,Requestor_Guid,Approver_Guid")] Scheduling scheduling)
        {

            if (ModelState.IsValid)
            {

                scheduling.Id = Guid.NewGuid();

                await this._context.Schedulings.InsertOneAsync(scheduling);

                return RedirectToAction(nameof(Index));

            }

            return View(scheduling);

        }

        // GET: Scheduling/Delete/5
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
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {

            if (this.SchedulingExists(id))
            {

                await this._context.Schedulings.DeleteOneAsync(s => s.Id == id);

            }

            return RedirectToAction(nameof(Index));

        }

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