using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Models
{
    public class ApplicationUserOTP
    {
        public int Id { get; set; }
        public int OTPNumber { get; set; }
        public bool Status { get; set; }
        public DateTime ValidTo { get; set; }
        public string ApplicationUserId { get; set; } = null!;
        public ApplicationUser ApplicationUser { get; set; } = null!;
    }
}
