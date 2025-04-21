
using System.Threading.Tasks;
using Nop.Services.Events;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Events;
using Nop.Web.Framework.Menu;

namespace Nop.Plugin.Misc.Suppliers
{
    public class EventConsumer : IConsumer<AdminMenuCreatedEvent>
    {
        private readonly IPermissionService _permissionService;

        public EventConsumer(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        public async Task HandleEventAsync(AdminMenuCreatedEvent eventMessage)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermission.Configuration.MANAGE_PLUGINS))
                return;

            // Use LINQ to find the Configuration node
            var configNode = eventMessage.RootMenuItem.ChildNodes
                .FirstOrDefault(node => node.SystemName == "Configuration");
            if (configNode != null)
                configNode.ChildNodes.Add(new AdminMenuItem
                {
                    SystemName = "Misc.Suppliers",
                    Title = "Suppliers",
                    Url = eventMessage.GetMenuItemUrl("SupplierAdmin", "List"),
                    IconClass = "far fa-dot-circle",
                    Visible = true,
                });
        }
    }
}