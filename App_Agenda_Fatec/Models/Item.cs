using System.ComponentModel.DataAnnotations;

using MongoDB.Bson.Serialization.Attributes;

namespace App_Agenda_Fatec.Models
{

    public class Item
    {

        [Required]
        [Display(Name = "Quantidade")]
        public int Quantity { get; set; }

        [Required]
        public Guid Equipment_Guid { get; set; }

        [BsonIgnore]
        [Display(Name = "Equipamento")]
        public Equipment? Equipment { get; set; } = null; // Valor padrão.

    }

}