using System.ComponentModel.DataAnnotations;

namespace App_Agenda_Fatec.Models
{

    public class Item
    {

        [Required]
        [Display(Name = "Quantidade")]
        public int Quantity { get; set; }

        [Required]
        [Display(Name = "Equipamento (GUID)")]
        public Guid Equipment_Guid { get; set; }

        [Display(Name = "Equipamento")]
        public Equipment? Equipment { get; set; } = null; // Valor padrão.

    }

}