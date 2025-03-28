using System.ComponentModel.DataAnnotations;

namespace UserManagementApp.Models {
    public class User {
        public int UserId { get; set; }
        [Required]
        public string Name { get; set; }=string.Empty;
        [Required]
        public string Email { get; set; }=string.Empty;
        [Required]
        public string Password { get; set; }=string.Empty;
        public DateTime? LastLoginTime { get; set; }
        public string Status { get; set; } = "active"; 
        public DateTime RegistrationTime { get; set; } = DateTime.Now;
    }
}


