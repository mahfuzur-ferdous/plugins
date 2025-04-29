using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.Suppliers.Areas.Admin.Models
{
    public record SupplierSelectionModel : BaseNopModel
    {
        public SupplierSelectionModel()
        {
            Suppliers = new List<SupplierModel>();
        }

        public int ProductId { get; set; }

        [NopResourceDisplayName("Admin.Suppliers.Fields.Supplier")]
        public int SelectedSupplierId { get; set; }

        public string SelectedSupplierName { get; set; }

        /// <summary>
        /// Gets or sets the list of available suppliers
        /// </summary>
        public IList<SupplierModel> Suppliers { get; set; }
    }
}

