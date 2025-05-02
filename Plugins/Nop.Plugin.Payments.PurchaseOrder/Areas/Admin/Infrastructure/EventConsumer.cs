using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Services.Events;
using Nop.Services.Security;
using Nop.Web.Framework.Events;
using Nop.Web.Framework.Menu;

namespace Nop.Plugin.Payments.PurchaseOrder.Areas.Admin.Infrastructure
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
            eventMessage.RootMenuItem.InsertBefore("Local plugins",
                new AdminMenuItem
                {
                    SystemName = "Payments.PurchaseOrder",
                    Title = "PurchaseOrder",
                    Url = eventMessage.GetMenuItemUrl("PurchaseOrder", "List"),
                    IconClass = "far fa-dot-circle",
                    Visible = true,
                });

        }
    }
}
