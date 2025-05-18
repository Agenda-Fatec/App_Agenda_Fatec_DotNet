using System.ComponentModel.DataAnnotations;

namespace App_Agenda_Fatec.Models
{

    public class Role
    {

        private Guid _id;

        public Guid Id { get { return this._id; } set { this._id = value; } }

        [Required]
        [Display(Name = "Nome")]
        public string? Name { get; set; }

        public bool? Active { get; set; } = true; // Valor padrão.

        [Display(Name = "Status")]
        public string? Activation_Stats { get; set; }

    }

}