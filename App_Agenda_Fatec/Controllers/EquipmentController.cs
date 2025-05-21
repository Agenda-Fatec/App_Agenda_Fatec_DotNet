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

namespace App_Agenda_Fatec.Controllers
{

    public class EquipmentController : Controller
    {

        private readonly MongoDBContext _context;

        public EquipmentController()
        {

            this._context = new MongoDBContext();

        }

        // GET: Equipment
        public async Task<IActionResult> Index()
        {

            List<Equipment> equipments = await this._context.Equipments.Find(FilterDefinition<Equipment>.Empty).ToListAsync();

            foreach (Equipment equipment in equipments)
            {

                equipment.Activation_Stats = (equipment.Active) ? "Ativado" : "Desativado";

            }

            return View(equipments);

        }

        // GET: Equipment/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {

            if (id == null)
            {

                return NotFound();

            }

            var equipment = await this._context.Equipments.Find(e => e.Id == id).FirstOrDefaultAsync();

            if (equipment == null)
            {

                return NotFound();

            }

            equipment.Activation_Stats = (equipment.Active) ? "Ativado" : "Desativado";

            return View(equipment);

        }

        // GET: Equipment/Create
        public IActionResult Create()
        {

            return View(new Equipment());

        }

        // POST: Equipment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Active")] Equipment equipment)
        {

            if (ModelState.IsValid)
            {

                equipment.Id = Guid.NewGuid();

                await this._context.Equipments.InsertOneAsync(equipment);

                return RedirectToAction(nameof(Index));

            }

            return View(equipment);

        }

        // GET: Equipment/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {

            if (id == null)
            {

                return NotFound();

            }

            var equipment = await this._context.Equipments.Find(e => e.Id == id).FirstOrDefaultAsync();

            if (equipment == null)
            {

                return NotFound();

            }

            return View(equipment);

        }

        // POST: Equipment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Description,Active")] Equipment equipment)
        {

            if (id != equipment.Id)
            {

                return NotFound();

            }

            if (ModelState.IsValid)
            {

                try
                {

                    await this._context.Equipments.ReplaceOneAsync(e => e.Id == id, equipment);

                }

                catch (DbUpdateConcurrencyException)
                {

                    if (!EquipmentExists(equipment.Id))
                    {

                        return NotFound();

                    }

                    else
                    {

                        throw;

                    }

                }

                return RedirectToAction(nameof(Index));

            }

            return View(equipment);

        }

        // GET: Equipment/ModifyActivation/5
        public async Task<IActionResult> ModifyActivation(Guid? id)
        {

            if (id == null)
            {

                return NotFound();

            }

            var equipment = await this._context.Equipments.Find(e => e.Id == id).FirstOrDefaultAsync();

            if (equipment == null)
            {

                return NotFound();

            }

            equipment.Activation_Stats = (equipment.Active) ? "Ativado" : "Desativado";

            return View(equipment);

        }

        // POST: Equipment/ModifyActivation/5
        [HttpPost, ActionName("ModifyActivation")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ModifyActivationConfirmed(Guid id)
        {

            var equipment = await this._context.Equipments.Find(e => e.Id == id).FirstOrDefaultAsync();

            if (equipment != null)
            {

                equipment.Active = !equipment.Active;

                await this._context.Equipments.ReplaceOneAsync(e => e.Id == id, equipment);

            }

            return RedirectToAction(nameof(Index));

        }

        private bool EquipmentExists(Guid id)
        {

            return this._context.Equipments.Find(e => e.Id == id).Any();

        }

    }

}