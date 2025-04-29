using System.Threading.Tasks;
using Nop.Plugin.Misc.Suppliers.Areas.Admin.Domain;
using Nop.Plugin.Misc.Suppliers.Areas.Admin.Models;

namespace Nop.Plugin.Misc.Suppliers.Areas.Admin.Factories
{
    /// <summary>
    /// Represents the supplier model factory
    /// </summary>
    public interface ISupplierModelFactory
    {
        /// <summary>
        /// Prepare supplier search model
        /// </summary>
        /// <param name="searchModel">Supplier search model</param>
        /// <returns>Supplier search model</returns>
        Task<SupplierSearchModel> PrepareSupplierSearchModelAsync(SupplierSearchModel searchModel);

        /// <summary>
        /// Prepare paged supplier list model
        /// </summary>
        /// <param name="searchModel">Supplier search model</param>
        /// <returns>Supplier list model</returns>
        Task<SupplierListModel> PrepareSupplierListModelAsync(SupplierSearchModel searchModel);

        /// <summary>
        /// Prepare supplier model
        /// </summary>
        /// <param name="model">Supplier model</param>
        /// <param name="supplier">Supplier</param>
        /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
        /// <returns>Supplier model</returns>
        Task<SupplierModel> PrepareSupplierModelAsync(SupplierModel model, Domain.Supplier supplier, bool excludeProperties = false);

        /// <summary>
        /// Prepare paged supplier list model with additional fields
        /// </summary>
        /// <param name="searchModel">Supplier search model</param>
        /// <returns>Supplier list model</returns>
        Task<SupplierListModel> PrepareExtendedSupplierListModelAsync(SupplierSearchModel searchModel);

        /// <summary>
        /// Insert or update product supplier mapping
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <param name="supplierId">Supplier identifier</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task InsertOrUpdateProductSupplierAsync(int productId, int supplierId);

        /// <summary>
        /// Get supplier identifier associated with a product
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <returns>Supplier identifier</returns>
        Task<int> GetProductSupplierIdAsync(int productId);
    }
}