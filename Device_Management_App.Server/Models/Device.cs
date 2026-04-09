using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Device_Management_App.Server.Models
{
    public class Device
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Manufacturer { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string OS { get; set; }

        [Required]
        public string OSVersion { get; set; }

        [Required]
        public string Processor { get; set; }

        [Required]
        public string RAMAmount { get; set; }

        public string? Description { get; set; }

        public int? AssignedUserId { get; set; }

        [ForeignKey("AssignedUserId")]
        public User? AssignedUser { get; set; }
    }
}
