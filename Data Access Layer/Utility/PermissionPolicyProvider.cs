using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

public class PermissionPolicyProvider : DefaultAuthorizationPolicyProvider
{
    public PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
        : base(options) { }

    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        // Checks if the Policy is exist
        var policy = await base.GetPolicyAsync(policyName);
        if (policy != null) return policy;

        // Checks if it's a save action
        if (policyName.Contains('|'))
        {
            var requiredPermissions = policyName.Split('|', StringSplitOptions.RemoveEmptyEntries);

            return new AuthorizationPolicyBuilder()
                .RequireAssertion(ctx =>
                    requiredPermissions.Any(p => ctx.User.HasClaim("Permission", p)))
                .Build();
        }

        // Exact match
        return new AuthorizationPolicyBuilder()
            .RequireClaim("Permission", policyName)
            .Build();
    }

}
