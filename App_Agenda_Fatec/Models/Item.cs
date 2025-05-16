using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;

namespace App_Agenda_Fatec.Models
{

    public class Item
    {

        private Guid _id;

        public Guid Id { get { return this._id; } set { this._id = value; } }

        [Required]
        [Display(Name = "Quantidade")]
        public int Quantity { get; set; }

        [NotMapped]
        [Display(Name = "Equipamento")]
        public Equipment? Equipment { get; set; }

    }

}