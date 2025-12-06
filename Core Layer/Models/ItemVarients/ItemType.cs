using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Models.ItemVarients
{
    public class ItemType
    {
        public int Id { get; set; }
        [Required]
        [RegularExpression(@"^(?!\s+$).*", ErrorMessage = "Name cannot be only whitespace.")]
        public string Name { get; set; } = null!;

        [RegularExpression(@"^(?!\s)(?!\s+$).+", ErrorMessage = "File name cannot start with spaces or be only whitespace.")]
        public string? Image { get; set; } = null!;
        public ICollection<Item> Items { get; set; } = new List<Item>();

        // Self-referencing FK (nullable for root categories)
        public int? ItemTypeId { get; set; }
        public ItemType? Parent { get; set; }                                       // Navigation to Parent
        public ICollection<ItemType> Children { get; set; } = new List<ItemType>(); // Navigation to children
    }
}
