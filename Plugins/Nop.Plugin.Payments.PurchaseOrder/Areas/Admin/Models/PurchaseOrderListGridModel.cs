using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Plugin.Payments.PurchaseOrder.Areas.Admin.Domain;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Payments.PurchaseOrder.Areas.Admin.Models
{
    /// <summary>
    /// Represents a purchase order list model (grid data)
    /// </summary>
    public record class PurchaseOrderListGridModel : BasePagedListModel<PurchaseOrderModel>
    {
    }
}
