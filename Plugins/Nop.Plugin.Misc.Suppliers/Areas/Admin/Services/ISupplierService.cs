using Nop.Core;
using Nop.Plugin.Misc.Suppliers.Areas.Admin.Domain;

namespace Nop.Plugin.Misc.Suppliers.Areas.Admin.Services
{
    /// <summary>
    /// Supplier service interface
    /// </summary>
    public interface ISupplierService
    {
        /// <summary>
        /// Gets a supplier by identifier
        /// </summary>
        /// <param name="supplierId">Supplier identifier</param>
        /// <returns>Supplier</returns>
        Task<Domain.Supplier> GetSupplierByIdAsync(int supplierId);

        /// <summary>
        /// Gets all suppliers (without paging)
        /// </summary>
        /// <returns>Suppliers</returns>
        Task<IList<Domain.Supplier>> GetAllSuppliersAsync();

        /// <summary>
        /// Gets all suppliers
        /// </summary>
        /// <param name="name">Filter by name</param>
        /// <param name="email">Filter by email</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Suppliers</returns>
        Task<IPagedList<Domain.Supplier>> GetAllSuppliersAsync(
            string name = "",
            string email = "",
            int pageIndex = 0,
            int pageSize = int.MaxValue,
            bool showHidden = false);

        /// <summary>
        /// Insert a supplier
        /// </summary>
        /// <param name="supplier">Supplier</param>
        Task InsertSupplierAsync(Domain.Supplier supplier);

        /// <summary>
        /// Update a supplier
        /// </summary>
        /// <param name="supplier">Supplier</param>
        Task UpdateSupplierAsync(Domain.Supplier supplier);

        /// <summary>
        /// Delete a supplier
        /// </summary>
        /// <param name="supplier">Supplier</param>
        Task DeleteSupplierAsync(Domain.Supplier supplier);

        /// <summary>
        /// Inserts or updates a product-supplier mapping
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <param name="supplierId">Supplier identifier</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task InsertOrUpdateProductSupplierMappingAsync(int productId, int supplierId);

        /// <summary>
        /// Gets the supplier ID associated with a product
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <returns>Supplier identifier, 0 if not found</returns>
        Task<int> GetProductSupplierIdAsync(int productId);
    }
}