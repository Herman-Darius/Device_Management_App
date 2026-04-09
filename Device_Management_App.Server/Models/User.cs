using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Device_Management_App.Server.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Role { get; set; }
        
        [Required]
        public string Location { get; set; }

        [JsonIgnore]
        public ICollection<Device> AssignedDevices { get; set; } = new List<Device>();
    }
}
