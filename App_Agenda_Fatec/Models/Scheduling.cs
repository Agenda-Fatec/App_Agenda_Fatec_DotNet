using System.ComponentModel.DataAnnotations;

using MongoDB.Bson.Serialization.Attributes;

namespace App_Agenda_Fatec.Models
{

    public class Scheduling
    {

        private Guid _id;

        public Guid Id { get { return this._id; } set { this._id = value; } }

        [Display(Name = "Data de Requisição")]
        public DateOnly Request_Date { get; set; } = DateOnly.Parse(DateTime.Now.ToString("dd/MM/yyyy")); // Valor padrão.

        [Display(Name = "Hora de Requisição")]
        public TimeOnly Request_Time { get; set; } = TimeOnly.Parse(DateTime.Now.ToString("HH:mm:ss")); // Valor padrão.

        [Required]
        [Display(Name = "Data de Utilização")]
        public DateOnly Utilization_Date { get; set; }

        [Required]
        [Display(Name = "Hora Inicial de Utilização")]
        public TimeOnly Start_Utilization_Time { get; set; }

        [Required]
        [Display(Name = "Hora Final de Utilização")]
        public TimeOnly End_Utilization_Time { get; set; }

        [Display(Name = "Situação Atual")]
        public string? Situation { get; set; } = "Pendente"; // Valor padrão.

        [Required]
        public Guid Room_Guid { get; set; }

        [BsonIgnore]
        [Display(Name = "Sala Requisitada")]
        public Room? Room { get; set; } = null; // Valor padrão.

        [Required]
        public Guid Requestor_Guid { get; set; }

        [BsonIgnore]
        [Display(Name = "Requisitor")]
        public User? Requestor { get; set; } = null; // Valor padrão.

        [Required]
        public Guid Approver_Guid { get; set; }

        [BsonIgnore]
        [Display(Name = "Aprovador (Responsável)")]
        public User? Approver { get; set; } = null; // Valor padrão.

    }

}