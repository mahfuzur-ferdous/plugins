using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Data;
using Nop.Plugin.Misc.Suppliers.Areas.Admin.Domain;
using Nop.Plugin.Payments.PurchaseOrder.Areas.Admin.Domain;
using Nop.Services.Catalog;
using Nop.Services.Logging;
using Nop.Services.Orders;

namespace Nop.Plugin.Payments.PurchaseOrder.Areas.Admin.Services;

/// <summary>
/// Purchase order service
/// </summary>
public class PurchaseOrderService : IPurchaseOrderService
{
    #region Fields

    private readonly IRepository<Domain.PurchaseOrder> _purchaseOrderRepository;
    private readonly IRepository<PurchaseOrderItem> _purchaseOrderItemRepository;
    private readonly IProductService _productService;
    private readonly ILogger _logger;
    private readonly IRepository<Product> _productRepository;
    private readonly IRepository<ProductSupplierMapping> _productSupplierMappingRepository;

    #endregion

    #region Ctor

    public PurchaseOrderService(
        IRepository<Domain.PurchaseOrder> purchaseOrderRepository,
        IRepository<PurchaseOrderItem> purchaseOrderItemRepository,
        IProductService productService,
        ILogger logger,
        IRepository<Product> productRepository,
        IRepository<ProductSupplierMapping> productSupplierMappingRepository)
    {
        _purchaseOrderRepository = purchaseOrderRepository;
        _purchaseOrderItemRepository = purchaseOrderItemRepository;
        _productService = productService;
        _logger = logger;
        _productRepository = productRepository;
        _productSupplierMappingRepository = productSupplierMappingRepository;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Get purchase order by id
    /// </summary>
    /// <param name="purchaseOrderId">Purchase order id</param>
    /// <returns>Purchase order</returns>
    public virtual async Task<Domain.PurchaseOrder> GetPurchaseOrderByIdAsync(int purchaseOrderId)
    {
        return await _purchaseOrderRepository.GetByIdAsync(purchaseOrderId);
    }

    /// <summary>
    /// Get all purchase orders
    /// </summary>
    /// <param name="supplierId">Supplier id; 0 to load all</param>
    /// <param name="createdFromUtc">Created date from (UTC); null to load all</param>
    /// <param name="createdToUtc">Created date to (UTC); null to load all</param>
    /// <param name="pageIndex">Page index</param>
    /// <param name="pageSize">Page size</param>
    /// <returns>Purchase orders</returns>
    public virtual async Task<IPagedList<Domain.PurchaseOrder>> GetAllPurchaseOrdersAsync(
        int supplierId = 0,
        DateTime? createdFromUtc = null,
        DateTime? createdToUtc = null,
        int pageIndex = 0,
        int pageSize = int.MaxValue)
    {
        var query = from po in _purchaseOrderRepository.Table
                    select po;

        if (supplierId > 0)
            query = query.Where(po => po.SupplierId == supplierId);

        if (createdFromUtc.HasValue)
            query = query.Where(po => po.CreatedOnUtc >= createdFromUtc.Value);

        if (createdToUtc.HasValue)
            query = query.Where(po => po.CreatedOnUtc <= createdToUtc.Value);

        query = query.OrderByDescending(po => po.CreatedOnUtc);

        return await query.ToPagedListAsync(pageIndex, pageSize);
    }

    /// <summary>
    /// Insert purchase order
    /// </summary>
    /// <param name="purchaseOrder">Purchase order</param>
    public virtual async Task InsertPurchaseOrderAsync(Domain.PurchaseOrder purchaseOrder)
    {
        if (purchaseOrder == null)
            throw new ArgumentNullException(nameof(purchaseOrder));

        await _purchaseOrderRepository.InsertAsync(purchaseOrder);
    }

    /// <summary>
    /// Update purchase order
    /// </summary>
    /// <param name="purchaseOrder">Purchase order</param>
    public virtual async Task UpdatePurchaseOrderAsync(Domain.PurchaseOrder purchaseOrder)
    {
        if (purchaseOrder == null)
            throw new ArgumentNullException(nameof(purchaseOrder));

        await _purchaseOrderRepository.UpdateAsync(purchaseOrder);
    }

    /// <summary>
    /// Delete purchase order
    /// </summary>
    /// <param name="purchaseOrder">Purchase order</param>
    public virtual async Task DeletePurchaseOrderAsync(Domain.PurchaseOrder purchaseOrder)
    {
        if (purchaseOrder == null)
            throw new ArgumentNullException(nameof(purchaseOrder));

        await _purchaseOrderRepository.DeleteAsync(purchaseOrder);
    }

    /// <summary>
    /// Get purchase order item by id
    /// </summary>
    /// <param name="purchaseOrderItemId">Purchase order item id</param>
    /// <returns>Purchase order item</returns>
    public virtual async Task<PurchaseOrderItem> GetPurchaseOrderItemByIdAsync(int purchaseOrderItemId)
    {
        return await _purchaseOrderItemRepository.GetByIdAsync(purchaseOrderItemId);
    }

    /// <summary>
    /// Get purchase order items by purchase order id
    /// </summary>
    /// <param name="purchaseOrderId">Purchase order id</param>
    /// <returns>Purchase order items</returns>
    public virtual async Task<IList<PurchaseOrderItem>> GetPurchaseOrderItemsByPurchaseOrderIdAsync(int purchaseOrderId)
    {
        var query = from poi in _purchaseOrderItemRepository.Table
                    where poi.PurchaseOrderId == purchaseOrderId
                    select poi;

        return await query.ToListAsync();
    }

    /// <summary>
    /// Insert purchase order item
    /// </summary>
    /// <param name="purchaseOrderItem">Purchase order item</param>
    public virtual async Task InsertPurchaseOrderItemAsync(PurchaseOrderItem purchaseOrderItem)
    {
        if (purchaseOrderItem == null)
            throw new ArgumentNullException(nameof(purchaseOrderItem));

        await _purchaseOrderItemRepository.InsertAsync(purchaseOrderItem);
    }

    /// <summary>
    /// Update purchase order item
    /// </summary>
    /// <param name="purchaseOrderItem">Purchase order item</param>
    public virtual async Task UpdatePurchaseOrderItemAsync(PurchaseOrderItem purchaseOrderItem)
    {
        if (purchaseOrderItem == null)
            throw new ArgumentNullException(nameof(purchaseOrderItem));

        await _purchaseOrderItemRepository.UpdateAsync(purchaseOrderItem);
    }

    /// <summary>
    /// Delete purchase order item
    /// </summary>
    /// <param name="purchaseOrderItem">Purchase order item</param>
    public virtual async Task DeletePurchaseOrderItemAsync(PurchaseOrderItem purchaseOrderItem)
    {
        if (purchaseOrderItem == null)
            throw new ArgumentNullException(nameof(purchaseOrderItem));

        await _purchaseOrderItemRepository.DeleteAsync(purchaseOrderItem);
    }

    /// <summary>
    /// Update product inventory based on purchase order
    /// </summary>
    /// <param name="purchaseOrder">Purchase order</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task UpdateInventoryAsync(Domain.PurchaseOrder purchaseOrder)
    {
        if (purchaseOrder == null)
            throw new ArgumentNullException(nameof(purchaseOrder));

        var purchaseOrderItems = await GetPurchaseOrderItemsByPurchaseOrderIdAsync(purchaseOrder.Id);
        foreach (var item in purchaseOrderItems)
        {
            var product = await _productService.GetProductByIdAsync(item.ProductId);
            if (product != null)
            {
                // Adjust the stock quantity
                product.StockQuantity += item.Quantity;
                await _productService.UpdateProductAsync(product);

                // Log the inventory change using ILogger
                await _logger.InformationAsync(
                    $"Stock quantity adjusted for product '{product.Name}' (ID: {product.Id}). " +
                    $"Added {item.Quantity} from Purchase Order #{purchaseOrder.PurchaseOrderNumber}",
                    customer: null);
            }
        }
    }



    #endregion
}