using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;

namespace App_Agenda_Fatec.Models
{

    public class Item
    {

        private Guid _id;

        public Guid Id { get { return this._id; } set { this._id = value; } }

        public int Quantity { get; set; }

        [Required]
        [Display(Name = "Equipment")]
        public Guid Equipment_Guid { get; set; }

        [NotMapped]
        [Display(Name = "Equipment Name")]
        public Equipment? Equipment { get; set; }

    }

}