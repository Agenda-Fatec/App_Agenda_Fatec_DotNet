using System.ComponentModel.DataAnnotations;

namespace App_Agenda_Fatec.Models
{

    public class Block
    {

        private Guid _id;

        [Display(Name = "ID")]
        public Guid Id { get { return this._id; } set { this._id = value; } }

        [Required]
        [Display(Name = "Nome")]
        public string? Name { get; set; }

        [Display(Name = "Descrição")]
        public string? Description { get; set; } = "Nenhuma descrição."; // Valor padrão.

        [Display(Name = "Status")]
        public bool? Active { get; set; } = true; // Valor padrão.

    }

}