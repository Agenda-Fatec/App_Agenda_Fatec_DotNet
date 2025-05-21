using System.ComponentModel.DataAnnotations;

using MongoDB.Bson.Serialization.Attributes;

namespace App_Agenda_Fatec.Models
{

    public class Room
    {

        private Guid _id;

        public Guid Id { get { return this._id; } set { this._id = value; } }

        [Required]
        [Display(Name = "Nome")]
        public string? Name { get; set; }

        [Required]
        [Display(Name = "Número")]
        public int Number { get; set; }

        [Display(Name = "Descrição")]
        public string? Description { get; set; } = "Nenhuma descrição."; // Valor padrão.

        [Display(Name = "Situação Atual")]
        public string? Situation { get; set; } = "Disponível"; // Valor padrão.

        public bool Active { get; set; } = true; // Valor padrão.

        [BsonIgnore]
        [Display(Name = "Status")]
        public string? Activation_Stats { get; set; }

        [Required]
        public Guid Block_Guid { get; set; }

        [BsonIgnore]
        [Display(Name = "Bloco")]
        public Block? Block { get; set; } = null; // Valor padrão.

        [Display(Name = "Itens")]
        public List<Item> Items { get; set; } = new List<Item>(); // Valor padrão.

    }

}