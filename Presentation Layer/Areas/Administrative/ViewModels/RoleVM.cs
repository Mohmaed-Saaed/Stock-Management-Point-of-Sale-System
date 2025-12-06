using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Areas.administrative.ViewModels
{
    public class RoleVM
    {
        public class RoleViewModel
        {
            public string? RoleId { get; set; }
            [Required]
            public string Name { get; set; } = null!;

            public List<int>? PermissionsIds { get; set; }

            // Root-level permissions (each with children)
            public List<PermissionTreeViewModel> PermissionsTree { get; set; } = new();
        }

        public class PermissionTreeViewModel
        {
            public int Id { get; set; }
            public string? Name { get; set; }       // e.g., "Stock"
            public string? EnglishName { get; set; }   // e.g., "Stock Management"
            public bool IsAssigned { get; set; }      // whether this role has it

            public List<PermissionTreeViewModel> Children { get; set; } = new();
        }

    }
}
