// Nop.Plugin.Misc.Suppliers/Models/SupplierListModel.cs
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.Suppliers.Models
{
    /// <summary>
    /// Represents a supplier list model
    /// </summary>
    public record SupplierListModel : BasePagedListModel<SupplierModel>
    {
    }
}