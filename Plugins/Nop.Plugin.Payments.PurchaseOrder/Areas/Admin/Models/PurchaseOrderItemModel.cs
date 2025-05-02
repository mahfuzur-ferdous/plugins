using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Payments.PurchaseOrder.Areas.Admin.Models
{
    /// <summary>
    /// Represents a purchase order item model
    /// </summary>
    public record class PurchaseOrderItemModel : BaseNopEntityModel
    {
        #region Properties

        public int PurchaseOrderId { get; set; }

        [NopResourceDisplayName("Plugin.Payments.PurchaseOrder.PurchaseOrderItem.Fields.Product")]
        public int ProductId { get; set; }

        [NopResourceDisplayName("Plugin.Payments.PurchaseOrder.PurchaseOrderItem.Fields.Product")]
        public string ProductName { get; set; }

        [NopResourceDisplayName("Plugin.Payments.PurchaseOrder.PurchaseOrderItem.Fields.Sku")]
        public string Sku { get; set; }

        [NopResourceDisplayName("Plugin.Payments.PurchaseOrder.PurchaseOrderItem.Fields.CurrentStock")]
        public int CurrentStock { get; set; }

        [NopResourceDisplayName("Plugin.Payments.PurchaseOrder.PurchaseOrderItem.Fields.Quantity")]
        public int Quantity { get; set; }

        [NopResourceDisplayName("Plugin.Payments.PurchaseOrder.PurchaseOrderItem.Fields.UnitCost")]
        public decimal UnitCost { get; set; }

        [NopResourceDisplayName("Plugin.Payments.PurchaseOrder.PurchaseOrderItem.Fields.TotalCost")]
        public decimal TotalCost { get; set; }

        [NopResourceDisplayName("Plugin.Payments.PurchaseOrder.PurchaseOrderItem.Fields.UnitCost")]
        public string FormattedUnitCost { get; set; }

        [NopResourceDisplayName("Plugin.Payments.PurchaseOrder.PurchaseOrderItem.Fields.TotalCost")]
        public string FormattedTotalCost { get; set; }

        #endregion
    }
}
