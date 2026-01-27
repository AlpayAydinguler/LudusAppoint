using Services.Contracts;

namespace LudusAppoint.Middleware
{
    public class TenantResolutionMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantResolutionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ITenantService tenantService)
        {
            // Extract host from request
            var host = context.Request.Host.Host;

            // Remove port if present
            var cleanHost = host.Split(':').First();

            // Get tenant by hostname
            var tenant = await tenantService.GetTenantByHostnameAsync(cleanHost);

            if (tenant != null)
            {
                // Store tenant in HttpContext for this request
                context.Items["CurrentTenant"] = tenant;
            }

            await _next(context);
        }
    }
}
