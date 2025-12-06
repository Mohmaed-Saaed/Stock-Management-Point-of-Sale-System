using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Models.Operations
{
    public class Transfer : Operation
    {
        [Required]
        public int FromBranchId { get; set; }   // FK Configured in AppDbContext
        public Branch FromBranch { get; set; } = null!;  // Source branch

        [Required]
        public int ToBranchId { get; set; }     // FK Configured in AppDbContext
        public Branch ToBranch { get; set; } = null!;    // Destination branch
    }
}
