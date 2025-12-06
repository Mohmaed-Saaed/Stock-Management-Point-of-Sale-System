using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Models
{
    public class ApplicationUser : IdentityUser
    {

        [NotMapped]
        public ICollection<Operation> Operations { get; set; } = new List<Operation>();

        // Cashier relationship (many-to-one)
        public int? BranchId { get; set; }  // Nullable for anything but cashiers
        public Branch? Branch { get; set; } = null!;  // Navigation to branch for cashiers
        public DateTime CreatedDate { get; set; }

        public ICollection<ApplicationUserOTP> ApplicationUserOTPs { get; set; } = new List<ApplicationUserOTP>();
    }
}
