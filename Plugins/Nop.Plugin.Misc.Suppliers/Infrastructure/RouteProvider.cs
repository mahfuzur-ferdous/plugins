

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Nop.Web.Framework.Mvc.Routing;

namespace Nop.Plugin.Misc.Suppliers.Infrastructure
{
    public class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder)
        {
            // Register your admin controller route
            endpointRouteBuilder.MapControllerRoute(
                name: "Plugin.Misc.Suppliers.Admin",
                pattern: "Admin/Suppliers/{action=List}/{id?}",
                defaults: new { controller = "SupplierAdmin", area = "Admin" });
        }

        public int Priority => 0;
    }
}

