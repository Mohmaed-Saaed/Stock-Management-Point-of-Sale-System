using CoreLayer.Models;
using InfrastructureLayer.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;


namespace InfrastructureLayer.Utility
{
    public class AppClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AppClaimsPrincipalFactory(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<IdentityOptions> optionsAccessor,
            IUnitOfWork unitOfWork)
            : base(userManager, roleManager, optionsAccessor)
        {
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);

            // roles
            var roles = await UserManager.GetRolesAsync(user);

            // permissions
            var role = await _roleManager.FindByNameAsync(roles.FirstOrDefault() ?? "");
            if (role != null)
            {
                var rolePermissions = (await _unitOfWork.RolePermissions
                    .GetAsync(r => r.RoleId == role.Id))
                    .Select(r => r.PermissionId);

                var permissions = (await _unitOfWork.Permissions
                    .GetAsync(p => rolePermissions.Contains(p.Id)))
                    .Select(x => x.Name);

                foreach (var perm in permissions.Distinct())
                {
                    identity.AddClaim(new Claim("Permission", perm));
                }
            }

            return identity;
        }
    }

}
