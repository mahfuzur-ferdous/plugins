using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.Suppliers.Areas.Admin.Models
{
    /// <summary>
    /// Represents a supplier search model
    /// </summary>
    public record SupplierSearchModel : BaseSearchModel
    {
        #region Properties

        [NopResourceDisplayName("Plugins.Misc.Suppliers.List.SearchName")]
        public string SearchName { get; set; }

        [NopResourceDisplayName("Plugins.Misc.Suppliers.List.SearchEmail")]
        public string SearchEmail { get; set; }

        #endregion
    }
}