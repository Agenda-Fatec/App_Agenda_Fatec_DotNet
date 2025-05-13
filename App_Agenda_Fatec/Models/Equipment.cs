using System.ComponentModel.DataAnnotations;

namespace App_Agenda_Fatec.Models
{

    public class Equipment
    {

        private Guid _id;

        public Guid Id { get { return this._id; } set { this._id = value; } }

        [Required]
        public string? Name { get; set; }


        public string? Description { get; set; } = "Nenhuma descrição."; // Valor padrão.


        [Display(Name = "Stats")]
        public bool Active { get; set; } = true; // Valor padrão.

    }

}