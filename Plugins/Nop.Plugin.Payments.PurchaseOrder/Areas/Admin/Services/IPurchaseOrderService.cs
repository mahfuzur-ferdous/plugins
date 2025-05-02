using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Data;
using Nop.Plugin.Payments.PurchaseOrder.Areas.Admin.Domain;
using Nop.Services.Catalog;

namespace Nop.Plugin.Payments.PurchaseOrder.Areas.Admin.Services
{
    /// <summary>
    /// Purchase order service interface
    /// </summary>
    public interface IPurchaseOrderService
    {
        /// <summary>
        /// Get purchase order by id
        /// </summary>
        /// <param name="purchaseOrderId">Purchase order id</param>
        /// <returns>Purchase order</returns>
        Task<Domain.PurchaseOrder> GetPurchaseOrderByIdAsync(int purchaseOrderId);

        /// <summary>
        /// Get all purchase orders
        /// </summary>
        /// <param name="supplierId">Supplier id; 0 to load all</param>
        /// <param name="createdFromUtc">Created date from (UTC); null to load all</param>
        /// <param name="createdToUtc">Created date to (UTC); null to load all</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Purchase orders</returns>
        Task<IPagedList<Domain.PurchaseOrder>> GetAllPurchaseOrdersAsync(
            int supplierId = 0,
            DateTime? createdFromUtc = null,
            DateTime? createdToUtc = null,
            int pageIndex = 0,
            int pageSize = int.MaxValue);

        /// <summary>
        /// Insert purchase order
        /// </summary>
        /// <param name="purchaseOrder">Purchase order</param>
        Task InsertPurchaseOrderAsync(Domain.PurchaseOrder purchaseOrder);

        /// <summary>
        /// Update purchase order
        /// </summary>
        /// <param name="purchaseOrder">Purchase order</param>
        Task UpdatePurchaseOrderAsync(Domain.PurchaseOrder purchaseOrder);

        /// <summary>
        /// Delete purchase order
        /// </summary>
        /// <param name="purchaseOrder">Purchase order</param>
        Task DeletePurchaseOrderAsync(Domain.PurchaseOrder purchaseOrder);

        /// <summary>
        /// Get purchase order item by id
        /// </summary>
        /// <param name="purchaseOrderItemId">Purchase order item id</param>
        /// <returns>Purchase order item</returns>
        Task<PurchaseOrderItem> GetPurchaseOrderItemByIdAsync(int purchaseOrderItemId);

        /// <summary>
        /// Get purchase order items by purchase order id
        /// </summary>
        /// <param name="purchaseOrderId">Purchase order id</param>
        /// <returns>Purchase order items</returns>
        Task<IList<PurchaseOrderItem>> GetPurchaseOrderItemsByPurchaseOrderIdAsync(int purchaseOrderId);

        /// <summary>
        /// Insert purchase order item
        /// </summary>
        /// <param name="purchaseOrderItem">Purchase order item</param>
        Task InsertPurchaseOrderItemAsync(PurchaseOrderItem purchaseOrderItem);

        /// <summary>
        /// Update purchase order item
        /// </summary>
        /// <param name="purchaseOrderItem">Purchase order item</param>
        Task UpdatePurchaseOrderItemAsync(PurchaseOrderItem purchaseOrderItem);

        /// <summary>
        /// Delete purchase order item
        /// </summary>
        /// <param name="purchaseOrderItem">Purchase order item</param>
        Task DeletePurchaseOrderItemAsync(PurchaseOrderItem purchaseOrderItem);

        /// <summary>
        /// Update product inventory based on purchase order
        /// </summary>
        /// <param name="purchaseOrder">Purchase order</param>
        Task UpdateInventoryAsync(Domain.PurchaseOrder purchaseOrder);
    }
}