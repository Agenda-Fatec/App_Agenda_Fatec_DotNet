using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;

namespace App_Agenda_Fatec.Models
{

    public class Scheduling
    {

        private Guid _id;

        public Guid Id { get; set; }

        public DateOnly Request_Date { get; set; } = DateOnly.Parse(DateTime.Now.ToString("dd/MM/yyyy"));

        public TimeOnly Request_Time { get; set; } = TimeOnly.Parse(DateTime.Now.ToString("HH:mm:ss"));

        [Required]
        public DateOnly Utilization_Date { get; set; }

        [Required]
        public TimeOnly Start_Utilization_Time { get; set; }

        [Required]
        public TimeOnly End_Utilization_Time { get; set; }

        public string? Situation { get; set; } = "Pendente";

        [Required]
        [Display(Name = "Room")]
        public Guid Room_Guid { get; set; }

        [NotMapped]
        [Display(Name = "Room Name")]
        public Room? Room { get; set; }

        [Required]
        [Display(Name = "Requestor")]
        public Guid Requestor_Guid { get; set; }

        [NotMapped]
        [Display(Name = "Requestor Name")]
        public User? Requestor { get; set; }

        [Required]
        [Display(Name = "Approver")]
        public Guid Approver_Guid { get; set; }

        [NotMapped]
        [Display(Name = "Approver Name")]
        public User? Approver { get; set; }

    }

}