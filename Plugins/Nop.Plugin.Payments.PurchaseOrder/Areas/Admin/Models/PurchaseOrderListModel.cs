using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Payments.PurchaseOrder.Areas.Admin.Models
{
    public record class PurchaseOrderListModel : BaseSearchModel
    {
        public PurchaseOrderListModel()
        {
            AvailableSuppliers = new List<SelectListItem>();
        }

        [NopResourceDisplayName("Plugin.Payments.PurchaseOrder.List.SearchSupplier")]
        public int SearchSupplierId { get; set; }

        [NopResourceDisplayName("Plugin.Payments.PurchaseOrder.List.SearchStartDate")]
        [UIHint("DateNullable")]
        public DateTime? SearchStartDate { get; set; }

        [NopResourceDisplayName("Plugin.Payments.PurchaseOrder.List.SearchEndDate")]
        [UIHint("DateNullable")]
        public DateTime? SearchEndDate { get; set; }

        public IList<SelectListItem> AvailableSuppliers { get; set; }
    }
}
