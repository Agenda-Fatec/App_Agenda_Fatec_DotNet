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

    public class BlockController : Controller
    {

        private readonly MongoDBContext _context;

        public BlockController()
        {

            this._context = new MongoDBContext();

        }

        // GET: Block
        public async Task<IActionResult> Index()
        {

            return View(await this._context.Blocks.Find(FilterDefinition<Block>.Empty).ToListAsync());

        }

        // GET: Block/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {

            if (id == null)
            {

                return NotFound();

            }

            var block = await this._context.Blocks.Find(b => b.Id == id).FirstOrDefaultAsync();

            if (block == null)
            {

                return NotFound();

            }

            ViewBag.Activation = (block.Active ?? false) ? "Ativado" : "Desativado";

            return View(block);

        }

        // GET: Block/Create
        public IActionResult Create()
        {

            return View(new Block());

        }

        // POST: Block/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Active")] Block block)
        {

            if (ModelState.IsValid)
            {

                block.Id = Guid.NewGuid();

                await this._context.Blocks.InsertOneAsync(block);

                return RedirectToAction(nameof(Index));

            }

            return View(block);

        }

        // GET: Block/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {

            if (id == null)
            {

                return NotFound();

            }

            var block = await this._context.Blocks.Find(b => b.Id == id).FirstOrDefaultAsync();

            if (block == null)
            {

                return NotFound();

            }

            return View(block);

        }

        // POST: Block/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Description,Active")] Block block)
        {

            if (id != block.Id)
            {

                return NotFound();

            }

            if (ModelState.IsValid)
            {

                try
                {

                    await this._context.Blocks.ReplaceOneAsync(b => b.Id == id, block);

                }

                catch (DbUpdateConcurrencyException)
                {

                    if (!BlockExists(block.Id))
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

            return View(block);

        }

        // GET: Block/Delete/5
        public async Task<IActionResult> ModifyActivation(Guid? id)
        {

            if (id == null)
            {

                return NotFound();

            }

            var block = await this._context.Blocks.Find(b => b.Id == id).FirstOrDefaultAsync();

            if (block == null)
            {

                return NotFound();

            }

            ViewBag.Activation = (block.Active ?? false) ? "Ativado" : "Desativado";

            return View(block);

        }

        // POST: Block/Delete/5
        [HttpPost, ActionName("ModifyActivation")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ModifyActivationConfirmed(Guid id)
        {

            var block = await this._context.Blocks.Find(b => b.Id == id).FirstOrDefaultAsync();

            if (block != null)
            {

                block.Active = !block.Active;

                await this._context.Blocks.ReplaceOneAsync(b => b.Id == id, block);

            }

            return RedirectToAction(nameof(Index));

        }

        private bool BlockExists(Guid id)
        {

            return this._context.Blocks.Find(b => b.Id == id).Any();

        }

    }

}