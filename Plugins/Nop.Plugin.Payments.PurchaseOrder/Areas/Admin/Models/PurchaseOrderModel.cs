using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Plugin.Payments.PurchaseOrder.Areas.Admin.Domain;
using Nop.Plugin.Payments.PurchaseOrder.Areas.Admin.Models;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Payments.PurchaseOrder.Areas.Admin.Models
{
    /// <summary>
    /// Represents a purchase order model
    /// </summary>
    public record class PurchaseOrderModel : BaseNopEntityModel
    {
        #region Ctor

        public PurchaseOrderModel()
        {
            AvailableSuppliers = new List<SelectListItem>();
            PurchaseOrderItems = new List<PurchaseOrderItemModel>();
        }

        #endregion

        #region Properties

        [NopResourceDisplayName("Plugin.Payments.PurchaseOrder.Fields.Supplier")]
        public int SupplierId { get; set; }

        [NopResourceDisplayName("Plugin.Payments.PurchaseOrder.Fields.Supplier")]
        public string SupplierName { get; set; }

        [NopResourceDisplayName("Plugin.Payments.PurchaseOrder.Fields.PurchaseOrderNumber")]
        public string PurchaseOrderNumber { get; set; }

        [NopResourceDisplayName("Plugin.Payments.PurchaseOrder.Fields.CreatedOn")]
        public DateTime CreatedOnUtc { get; set; }

        [NopResourceDisplayName("Plugin.Payments.PurchaseOrder.Fields.CreatedOn")]
        public string CreatedOn { get; set; }

        [NopResourceDisplayName("Plugin.Payments.PurchaseOrder.Fields.Status")]
        public int PurchaseOrderStatusId { get; set; }

        [NopResourceDisplayName("Plugin.Payments.PurchaseOrder.Fields.Status")]
        public string PurchaseOrderStatus { get; set; }

        [NopResourceDisplayName("Plugin.Payments.PurchaseOrder.Fields.CreatedBy")]
        public int CreatedByCustomerId { get; set; }

        [NopResourceDisplayName("Plugin.Payments.PurchaseOrder.Fields.CreatedBy")]
        public string CreatedByCustomerName { get; set; }

        [NopResourceDisplayName("Plugin.Payments.PurchaseOrder.Fields.TotalAmount")]
        public decimal TotalAmount { get; set; }

        [NopResourceDisplayName("Plugin.Payments.PurchaseOrder.Fields.TotalAmount")]
        public string FormattedTotalAmount { get; set; }

        public IList<SelectListItem> AvailableSuppliers { get; set; }

        public IList<PurchaseOrderItemModel> PurchaseOrderItems { get; set; }
        public IList<SelectListItem> AvailableStatuses { get; set; }

        #endregion
    }
}

