using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.Suppliers.Areas.Admin.Models
{
    /// <summary>
    /// Represents a supplier list model
    /// </summary>
    public record SupplierListModel : BasePagedListModel<SupplierModel>
    {
    }
}