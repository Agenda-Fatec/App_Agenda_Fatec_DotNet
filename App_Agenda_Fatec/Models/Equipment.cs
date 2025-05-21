using System.ComponentModel.DataAnnotations;

using MongoDB.Bson.Serialization.Attributes;

namespace App_Agenda_Fatec.Models
{

    public class Equipment
    {

        private Guid _id;

        [Display(Name = "ID")]
        public Guid Id { get { return this._id; } set { this._id = value; } }

        [Required]
        [Display(Name = "Nome")]
        public string? Name { get; set; }

        [Display(Name = "Descrição")]
        public string? Description { get; set; } = "Nenhuma descrição."; // Valor padrão.

        public bool Active { get; set; } = true; // Valor padrão.

        [BsonIgnore]
        [Display(Name = "Status")]
        public string? Activation_Stats { get; set; }

    }

}