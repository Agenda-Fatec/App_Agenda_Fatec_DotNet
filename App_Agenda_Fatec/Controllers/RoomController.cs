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
using System.ComponentModel.DataAnnotations;

namespace App_Agenda_Fatec.Controllers
{

    public class RoomController : Controller
    {

        private readonly MongoDBContext _context;

        public RoomController()
        {

            this._context = new MongoDBContext();

        }

        // GET: Room/Available
        public async Task<IActionResult> Available()
        {

            return View(await this._context.Rooms.Find(r => r.Active ?? false).ToListAsync());

        }

        // GET: Room
        public async Task<IActionResult> Index()
        {

            return View(await this._context.Rooms.Find(FilterDefinition<Room>.Empty).ToListAsync());

        }

        // GET: Room/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {

            if (id == null)
            {

                return NotFound();

            }

            var room = await this._context.Rooms.Find(r => r.Id == id).FirstOrDefaultAsync();

            if (room == null)
            {

                return NotFound();

            }

            ViewBag.Activation = (room.Active ?? false) ? "Ativado" : "Desativado";

            return View(room);

        }

        // GET: Room/Create
        public async Task<IActionResult> Create()
        {

            ViewBag.Blocks = await this._context.Blocks.Find(b => b.Active ?? false).ToListAsync();

            return View(new Room());

        }

        // POST: Room/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Number,Description,Situation,Active")] Room room, [Required] Guid block_guid)
        {

            if (ModelState.IsValid)
            {

                room.Id = Guid.NewGuid();

                Block room_block = await this._context.Blocks.Find(b => b.Id == block_guid).FirstOrDefaultAsync();

                room_block.Active = null; // Esse campo não é necessário no contexto atual.

                room.Block = room_block;

                await this._context.Rooms.InsertOneAsync(room);

                return RedirectToAction(nameof(Index));

            }

            return View(room);

        }

        // GET: Room/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {

            if (id == null)
            {

                return NotFound();

            }

            var room = await this._context.Rooms.Find(r => r.Id == id).FirstOrDefaultAsync();

            if (room == null)
            {

                return NotFound();

            }

            ViewBag.Blocks = await this._context.Blocks.Find(FilterDefinition<Block>.Empty).ToListAsync();

            return View(room);

        }

        // POST: Room/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Number,Description,Situation,Active")] Room room, [Required] Guid block_guid)
        {

            if (id != room.Id)
            {

                return NotFound();

            }

            if (ModelState.IsValid)
            {

                try
                {

                    Block room_block = await this._context.Blocks.Find(b => b.Id == block_guid).FirstOrDefaultAsync();

                    room_block.Active = null; // Esse campo não é necessário no contexto atual.

                    room.Block = room_block;

                    await this._context.Rooms.ReplaceOneAsync(r => r.Id == id, room);

                }

                catch (DbUpdateConcurrencyException)
                {

                    if (!RoomExists(room.Id))
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

            return View(room);

        }

        // GET: Room/Delete/5
        public async Task<IActionResult> ModifyActivation(Guid? id)
        {

            if (id == null)
            {

                return NotFound();

            }

            var room = await this._context.Rooms.Find(r => r.Id == id).FirstOrDefaultAsync();

            if (room == null)
            {

                return NotFound();

            }

            ViewBag.Activation = (room.Active ?? false) ? "Ativado" : "Desativado";

            return View(room);

        }

        // POST: Room/Delete/5
        [HttpPost, ActionName("ModifyActivation")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ModifyActivationConfirmed(Guid id)
        {

            var room = await this._context.Rooms.Find(r => r.Id == id).FirstOrDefaultAsync();

            if (room != null)
            {

                room.Active = !room.Active;

                await this._context.Rooms.ReplaceOneAsync(r => r.Id == id, room);

            }

            return RedirectToAction(nameof(Index));

        }

        private bool RoomExists(Guid id)
        {

            return this._context.Rooms.Find(r => r.Id == id).Any();

        }

    }

}