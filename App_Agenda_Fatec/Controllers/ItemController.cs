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

using Microsoft.AspNetCore.Authorization;

namespace App_Agenda_Fatec.Controllers
{

    [Authorize(Roles = "Admin")]
    public class ItemController : Controller
    {

        private readonly MongoDBContext _context;

        public ItemController()
        {

            this._context = new MongoDBContext();

        }

        // GET: Item/Add
        public async Task<IActionResult> Add(Guid room_guid)
        {

            ViewBag.Room = room_guid;

            ViewBag.Equipments = await this._context.Equipments.Find(e => e.Active).ToListAsync();

            return View(new Item());

        }

        // POST: Item/Add
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add([Bind("Id,Quantity,Equipment_Guid")] Item item, [Required] Guid room_guid, [Required] Guid equipment_guid)
        {

            if (ModelState.IsValid)
            {

                Room room = await this._context.Rooms.Find(r => r.Id == room_guid).FirstOrDefaultAsync();

                if (this.ItemExists(room.Items, equipment_guid))
                {

                    foreach (Item room_item in room.Items)
                    {

                        if (room_item.Equipment_Guid == equipment_guid)
                        {

                            room_item.Quantity += item.Quantity;

                            break;

                        }

                    }

                }

                else
                {

                    room.Items.Add(item);

                }

                await this._context.Rooms.ReplaceOneAsync(r => r.Id == room_guid, room);

                return RedirectToAction("Details", "Room", new { id = room_guid });

            }

            return View(item);

        }

        // GET: Item/Remove
        public async Task<IActionResult> Remove(Guid? room_guid, Guid? equipment_guid)
        {

            if (room_guid == null || equipment_guid == null)
            {

                return NotFound();

            }

            var room = await this._context.Rooms.Find(r => r.Id == room_guid).FirstOrDefaultAsync();

            Item? item = room.Items.Find(i => i.Equipment_Guid == equipment_guid);

            if (item == null)
            {

                return NotFound();

            }

            ViewBag.Room = room_guid;

            item.Equipment = await this._context.Equipments.Find(e => e.Id == equipment_guid).FirstOrDefaultAsync();

            return View(item);

        }

        // POST: Item/Remove
        [HttpPost, ActionName("Remove")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveConfirmed(Guid room_guid, Guid equipment_guid)
        {

            var room = await this._context.Rooms.Find(r => r.Id == room_guid).FirstOrDefaultAsync();

            if (room != null)
            {

                foreach (Item room_item in room.Items)
                {

                    if (room_item.Equipment_Guid == equipment_guid)
                    {

                        if (!room.Items.Remove(room_item))
                        {

                            ModelState.AddModelError("", "Erro ao tentar remover este item.");

                            return View(room_item);

                        }

                        break;

                    }

                }

                await this._context.Rooms.ReplaceOneAsync(r => r.Id == room_guid, room);

            }

            return RedirectToAction("Details", "Room", new { id = room_guid });

        }

        private bool ItemExists(List<Item> items, Guid equipment_guid)
        {

            return items.Any(i => i.Equipment_Guid == equipment_guid);

        }

    }

}