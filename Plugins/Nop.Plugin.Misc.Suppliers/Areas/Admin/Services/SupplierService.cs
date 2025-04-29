using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Data;
using Nop.Plugin.Misc.Suppliers.Areas.Admin.Domain;

namespace Nop.Plugin.Misc.Suppliers.Areas.Admin.Services
{
    /// <summary>
    /// Supplier service
    /// </summary>
    public class SupplierService : ISupplierService
    {
        #region Fields
        private readonly IRepository<Domain.Supplier> _supplierRepository;
        private readonly IRepository<ProductSupplierMapping> _productSupplierMapping;
        private readonly IStaticCacheManager _staticCacheManager;
        #endregion

        #region Ctor
        public SupplierService(
            IRepository<Domain.Supplier> supplierRepository,
            IRepository<ProductSupplierMapping> productSupplierMapping,
            IStaticCacheManager staticCacheManager)
        {
            _supplierRepository = supplierRepository;
            _productSupplierMapping = productSupplierMapping;
            _staticCacheManager = staticCacheManager;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets a supplier by id
        /// </summary>
        /// <param name="supplierId">Supplier identifier</param>
        /// <returns>Supplier</returns>
        public virtual async Task<Domain.Supplier> GetSupplierByIdAsync(int supplierId)
        {
            return await _supplierRepository.GetByIdAsync(supplierId);
        }

        /// <summary>
        /// Gets all suppliers
        /// </summary>
        /// <returns>Suppliers</returns>
        public virtual async Task<IList<Domain.Supplier>> GetAllSuppliersAsync()
        {
            return await _supplierRepository.Table.ToListAsync();
        }

        /// <summary>
        /// Gets all suppliers
        /// </summary>
        /// <param name="name">Filter by name</param>
        /// <param name="email">Filter by email</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Suppliers</returns>
        public virtual async Task<IPagedList<Domain.Supplier>> GetAllSuppliersAsync(
            string name = "",
            string email = "",
            int pageIndex = 0,
            int pageSize = int.MaxValue,
            bool showHidden = false)
        {
            name = string.IsNullOrWhiteSpace(name) ? null : name.Trim();
            email = string.IsNullOrWhiteSpace(email) ? null : email.Trim();

            var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(
                SupplierDefaults.AdminSupplierAllModelKey, 
                name, email, pageIndex, pageSize, showHidden);

            var allSuppliers = await _staticCacheManager.GetAsync(cacheKey, async () =>
            {
                var query = _supplierRepository.Table;

                if (!string.IsNullOrEmpty(name))
                    query = query.Where(s => s.Name.Contains(name));

                if (!string.IsNullOrEmpty(email))
                    query = query.Where(s => s.Email.Contains(email));

                if (!showHidden)
                    query = query.Where(s => s.Active);

                return await query.OrderBy(s => s.Name).ToListAsync();
            });

            return new PagedList<Domain.Supplier>(allSuppliers, pageIndex, pageSize);
        }

        /// <summary>
        /// Insert a supplier
        /// </summary>
        /// <param name="supplier">Supplier</param>
        public virtual async Task InsertSupplierAsync(Domain.Supplier supplier)
        {
            if (supplier == null)
                throw new ArgumentNullException(nameof(supplier));

            supplier.CreatedOnUtc = DateTime.UtcNow;
            await _supplierRepository.InsertAsync(supplier);
            await _staticCacheManager.RemoveByPrefixAsync(SupplierDefaults.AdminSupplierAllPrefixCacheKey);
        }

        /// <summary>
        /// Update a supplier
        /// </summary>
        /// <param name="supplier">Supplier</param>
        public virtual async Task UpdateSupplierAsync(Domain.Supplier supplier)
        {
            if (supplier == null)
                throw new ArgumentNullException(nameof(supplier));

            await _supplierRepository.UpdateAsync(supplier);
            await _staticCacheManager.RemoveByPrefixAsync(SupplierDefaults.AdminSupplierAllPrefixCacheKey);
        }

        /// <summary>
        /// Delete a supplier
        /// </summary>
        /// <param name="supplier">Supplier</param>
        public virtual async Task DeleteSupplierAsync(Domain.Supplier supplier)
        {
            if (supplier == null)
                throw new ArgumentNullException(nameof(supplier));

            await _supplierRepository.DeleteAsync(supplier);
            await _staticCacheManager.RemoveByPrefixAsync(SupplierDefaults.AdminSupplierAllPrefixCacheKey);
        }

        /// <summary>
        /// Insert a supplier (alternative implementation)
        /// </summary>
        /// <param name="supplier">Supplier entity</param>
        public virtual async Task InsertAsync(Domain.Supplier supplier)
        {
            if (supplier == null)
                throw new ArgumentNullException(nameof(supplier));

            supplier.CreatedOnUtc = DateTime.UtcNow;
            await _supplierRepository.InsertAsync(supplier);
            await _staticCacheManager.RemoveByPrefixAsync(SupplierDefaults.AdminSupplierAllPrefixCacheKey);
        }

        /// <summary>
        /// Update a supplier (alternative implementation)
        /// </summary>
        /// <param name="supplier">Supplier entity</param>
        public virtual async Task UpdateAsync(Domain.Supplier supplier)
        {
            if (supplier == null)
                throw new ArgumentNullException(nameof(supplier));

            await _supplierRepository.UpdateAsync(supplier);
            await _staticCacheManager.RemoveByPrefixAsync(SupplierDefaults.AdminSupplierAllPrefixCacheKey);
        }

        /// <summary>
        /// Delete a supplier (alternative implementation)
        /// </summary>
        /// <param name="supplier">Supplier entity</param>
        public virtual async Task DeleteAsync(Domain.Supplier supplier)
        {
            if (supplier == null)
                throw new ArgumentNullException(nameof(supplier));

            await _supplierRepository.DeleteAsync(supplier);
            await _staticCacheManager.RemoveByPrefixAsync(SupplierDefaults.AdminSupplierAllPrefixCacheKey);
        }

        /// <summary>
        /// Inserts or updates a product-supplier mapping
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <param name="supplierId">Supplier identifier</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task InsertOrUpdateProductSupplierMappingAsync(int productId, int supplierId)
        {
            var existing = await _productSupplierMapping.Table
                .FirstOrDefaultAsync(x => x.ProductId == productId);

            if (existing != null)
            {
                existing.SupplierId = supplierId;
                await _productSupplierMapping.UpdateAsync(existing);
            }
            else
            {
                var newMapping = new ProductSupplierMapping
                {
                    ProductId = productId,
                    SupplierId = supplierId
                };
                await _productSupplierMapping.InsertAsync(newMapping);
            }
        }

        /// <summary>
        /// Gets the supplier ID associated with a product
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <returns>Supplier identifier, 0 if not found</returns>
        public virtual async Task<int> GetProductSupplierIdAsync(int productId)
        {
            var existing = await _productSupplierMapping.Table
                .FirstOrDefaultAsync(x => x.ProductId == productId);

            return existing?.SupplierId ?? 0;
        }
        #endregion
    }
}