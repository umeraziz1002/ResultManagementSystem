using Microsoft.AspNetCore.Identity;

namespace ResultManagementSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public string? CNIC { get; set; }

        // Academic Info (for Students)
        public int? BatchId { get; set; }
        public Batch? Batch { get; set; }

        public bool IsActive { get; set; } = true;
    }
}