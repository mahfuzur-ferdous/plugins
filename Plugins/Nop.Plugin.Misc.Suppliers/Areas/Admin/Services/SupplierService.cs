using Nop.Core;
using Nop.Data;

namespace Nop.Plugin.Misc.Suppliers.Areas.Admin.Services
{
    /// <summary>
    /// Supplier service
    /// </summary>
    public class SupplierService : ISupplierService
    {
        #region Fields

        private readonly IRepository<Domain.Supplier> _supplierRepository;

        #endregion

        #region Ctor

        public SupplierService(IRepository<Domain.Supplier> supplierRepository)
        {
            _supplierRepository = supplierRepository;
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
        /// <param name="name">Filter by name</param>
        /// <param name="email">Filter by email</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Suppliers</returns>
        public async Task<IPagedList<Domain.Supplier>> GetAllSuppliersAsync(string name, string email,
            int pageIndex, int pageSize)
        {
            var query = _supplierRepository.Table;

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(s => s.Name.Contains(name));

            if (!string.IsNullOrWhiteSpace(email))
                query = query.Where(s => s.Email.Contains(email));

            query = query.OrderBy(s => s.Name);

            return await query.ToPagedListAsync(pageIndex, pageSize);
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
        }
        public Task<IPagedList<Domain.Supplier>> GetAllSuppliersAsync(string name = "", string email = "", int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}