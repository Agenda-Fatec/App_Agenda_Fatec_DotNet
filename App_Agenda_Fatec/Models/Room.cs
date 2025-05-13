using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;

namespace App_Agenda_Fatec.Models
{

    public class Room
    {

        private Guid _id;

        public Guid Id { get { return this._id; } set { this._id = value; } }

        [Required]
        public string? Name { get; set; }

        [Required]
        public int Number { get; set; }

        public string? Description { get; set; } = "Nenhuma descrição."; // Valor padrão.

        public string? Situation { get; set; } = "Disponível"; // Valor padrão.

        [Display(Name = "Stats")]
        public bool Active { get; set; } = true; // Valor padrão.

        [Required]
        [Display(Name = "Block")]
        public Guid Block_Guid { get; set; }

        [NotMapped]
        [Display(Name = "Block Name")]
        public Block? Block { get; set; }

        [Required]
        [Display(Name = "Items")]
        public List<Guid> Items_Guids { get; set; } = new List<Guid>(); // Valor padrão.

        [NotMapped]
        [Display(Name = "Items Names")]
        public List<Item> Items { get; set; } = new List<Item>(); // Valor padrão.

    }

}