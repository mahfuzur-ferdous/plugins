using Nop.Plugin.Misc.Suppliers.Areas.Admin.Models;
using Nop.Plugin.Misc.Suppliers.Areas.Admin.Domain;

namespace Nop.Plugin.Misc.Suppliers.Areas.Admin.Factories
{
    /// <summary>
    /// Represents the supplier model factory
    /// </summary>
    public interface ISupplierModelFactory
    {
        ///// <summary>
        ///// Prepare supplier search model
        ///// </summary>
        ///// <param name="searchModel">Supplier search model</param>
        ///// <returns>Supplier search model</returns>
        //Task<SupplierSearchModel> PrepareSupplierSearchModelAsync(SupplierSearchModel searchModel);

        ///// <summary>
        ///// Prepare paged supplier list model
        ///// </summary>
        ///// <param name="searchModel">Supplier search model</param>
        ///// <returns>Supplier list model</returns>
        //Task<SupplierListModel> PrepareSupplierListModelAsync(SupplierSearchModel searchModel);

        /// <summary>
        /// Prepare supplier model
        /// </summary>
        /// <param name="model">Supplier model</param>
        /// <param name="supplier">Supplier</param>
        /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
        /// <returns>Supplier model</returns>
        Task<SupplierModel> PrepareSupplierModelAsync(SupplierModel model, Supplier supplier, bool excludeProperties = false);


    }
}
