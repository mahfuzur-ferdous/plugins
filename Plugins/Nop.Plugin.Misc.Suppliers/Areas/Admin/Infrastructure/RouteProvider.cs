using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Nop.Web.Framework.Mvc.Routing;

namespace Nop.Plugin.Misc.Suppliers.Areas.Admin.Infrastructure
{
    public class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapControllerRoute(
                name: "Plugin.Misc.Suppliers.Admin",
                pattern: "Admin/SupplierAdmin/{action=List}/{id?}",
                defaults: new { controller = "SupplierAdmin", area = "Admin" });
        }
        public int Priority => 0;
    }
}

