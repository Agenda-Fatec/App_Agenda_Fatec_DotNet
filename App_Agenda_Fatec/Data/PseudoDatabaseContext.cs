using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using App_Agenda_Fatec.Models;

namespace App_Agenda_Fatec.Data
{

    // "Falso" arquivo de contexto de banco de dados, necessário para utilizar o Scaffolder.

    public class PseudoDatabaseContext : DbContext
    {

        public PseudoDatabaseContext (DbContextOptions<PseudoDatabaseContext> options) : base(options) {  }

        // Tabelas.

        public DbSet<Models.Block> Blocks { get; set; } = default!;

        public DbSet<Models.Equipment> Equipments { get; set; } = default!;

        public DbSet<Models.Item> Items { get; set; } = default!;

        public DbSet<Models.Role> Roles { get; set; } = default!;

        public DbSet<Models.Room> Rooms { get; set; } = default!;

        public DbSet<Models.Scheduling> Schedulings { get; set; } = default!;

        public DbSet<Models.User> Users { get; set; } = default!;

    }

}