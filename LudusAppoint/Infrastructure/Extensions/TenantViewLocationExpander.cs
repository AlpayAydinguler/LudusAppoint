using Microsoft.AspNetCore.Mvc.Razor;

namespace LudusAppoint.Infrastructure.Extensions
{
    public class TenantViewLocationExpander : IViewLocationExpander
    {
        public void PopulateValues(ViewLocationExpanderContext context)
        {
            // Get tenant from HttpContext
            var httpContext = context.ActionContext.HttpContext;
            if (httpContext.Items.TryGetValue("CurrentTenant", out var tenantObj))
            {
                var tenant = tenantObj as Entities.Models.Tenant;
                context.Values["tenant"] = tenant?.Name;
            }
        }

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            // Get HttpContext from the ActionContext
            var httpContext = context.ActionContext.HttpContext;

            // Check if tenant is set in HttpContext.Items (set by middleware)
            if (httpContext.Items.TryGetValue("CurrentTenant", out var tenantObj))
            {
                var tenant = tenantObj as Entities.Models.Tenant;
                var tenantName = tenant?.Name;

                if (!string.IsNullOrEmpty(tenantName))
                {
                    // First look in tenant-specific folder, then fall back to default
                    var expandedLocations = new List<string>();

                    // Tenant-specific views (e.g., Views/Company1/...)
                    expandedLocations.Add($"/Views/{tenantName}/{{1}}/{{0}}.cshtml");
                    expandedLocations.Add($"/Views/{tenantName}/Shared/{{0}}.cshtml");

                    // Default views
                    expandedLocations.AddRange(viewLocations);

                    return expandedLocations;
                }
            }

            // If no tenant found, return default view locations
            return viewLocations;
        }
    }
}
