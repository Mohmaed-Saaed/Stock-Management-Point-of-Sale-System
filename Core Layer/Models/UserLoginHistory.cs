using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Models
{
    public class UserLoginHistory
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public DateTime LoginTime { get; set; }
        public DateTime? LogoutTime { get; set; }
        public string IPAddress { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
    }
}
