using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;


namespace Nop.Plugin.Payments.PurchaseOrder.Areas.Admin.Models
{
    /// <summary>
    /// Represents a product model for purchase order
    /// </summary>
    public record class ProductModel : BaseNopEntityModel
    {
        #region Properties

        public string Name { get; set; }

        public string Sku { get; set; }

        public int StockQuantity { get; set; }

        #endregion
    }
}
