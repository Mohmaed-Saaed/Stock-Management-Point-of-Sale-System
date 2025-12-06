using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Models.ItemVarients
{
    public class TargetAudience
    {
        public int Id { get; set; }
        [Required]
        [RegularExpression(@"^(?!\s+$).*", ErrorMessage = "Name cannot be only whitespace.")]
        public string Name { get; set; } = null!;

        [RegularExpression(@"^(?!\s)(?!\s+$).+", ErrorMessage = "File name cannot start with spaces or be only whitespace.")]
        public string? Image { get; set; } = null!;
        public ICollection<Item> Items { get; set; } = new List<Item>();
    }
}
