using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;
using Nop.Data;
using Nop.Plugin.Misc.Suppliers.Areas.Admin.Domain;
using Nop.Plugin.Misc.Suppliers.Areas.Admin.Services;
using Nop.Plugin.Payments.PurchaseOrder.Areas.Admin.Domain;
using Nop.Plugin.Payments.PurchaseOrder.Areas.Admin.Services;
using Nop.Plugin.Payments.PurchaseOrder.Areas.Admin.Models;
using Nop.Services.Catalog;
using Nop.Services.Customers;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Areas.Admin.Controllers;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Web.Framework.Models.Extensions;

namespace Nop.Plugin.Payments.PurchaseOrder.Areas.Admin.Controllers
{
    [AuthorizeAdmin]
    [Area("Admin")]
    public class PurchaseOrderController : BaseController
    {
        #region Fields

        private readonly IPermissionService _permissionService;
        private readonly IPurchaseOrderService _purchaseOrderService;
        private readonly IProductService _productService;
        private readonly ILocalizationService _localizationService;
        private readonly INotificationService _notificationService;
        private readonly ICustomerService _customerService;
        private readonly IWorkContext _workContext;
        private readonly ILogger _logger;
        private readonly ISupplierService _supplierService;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<ProductSupplierMapping> _productSupplierMappingRepository;
        private readonly IDateTimeHelper _dateTimeHelper;

        #endregion

        #region Ctor

        public PurchaseOrderController(
            IPermissionService permissionService,
            IPurchaseOrderService purchaseOrderService,
            IProductService productService,
            ILocalizationService localizationService,
            INotificationService notificationService,
            ICustomerService customerService,
            IWorkContext workContext,
            ISupplierService supplierService,
            ILogger logger,
            IRepository<Product> productRepository,
            IRepository<ProductSupplierMapping> productSupplierMappingRepository,
            IDateTimeHelper dateTimeHelper)
        {
            _permissionService = permissionService;
            _purchaseOrderService = purchaseOrderService;
            _productService = productService;
            _localizationService = localizationService;
            _notificationService = notificationService;
            _customerService = customerService;
            _workContext = workContext;
            _supplierService = supplierService;
            _logger = logger;
            _productRepository = productRepository;
            _productSupplierMappingRepository = productSupplierMappingRepository;
            _dateTimeHelper = dateTimeHelper;
        }
        #endregion

        #region Utilities

        protected virtual async Task PrepareSupplierListAsync(PurchaseOrderModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var suppliers = await _supplierService.GetAllSuppliersAsync();
            model.AvailableSuppliers = suppliers.Select(s => new SelectListItem
            {
                Text = s.Name,
                Value = s.Id.ToString()
            }).ToList();
        }

        protected virtual async Task PrepareSupplierListModelAsync(PurchaseOrderListModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var suppliers = await _supplierService.GetAllSuppliersAsync();
            model.AvailableSuppliers = suppliers.Select(s => new SelectListItem
            {
                Text = s.Name,
                Value = s.Id.ToString()
            }).ToList();

            // Add an "All" option
            model.AvailableSuppliers.Insert(0, new SelectListItem
            {
                Text = await _localizationService.GetResourceAsync("Admin.Common.All"),
                Value = "0"
            });
        }

        protected virtual async Task<List<ProductModel>> GetProductsBySupplierIdAsync(int supplierId)
        {
            if (supplierId == 0)
                return new List<ProductModel>();

            // Get products by supplier ID using the supplier service
            var products = await _supplierService.GetProductsBySupplierId(supplierId);

            var productModels = new List<ProductModel>();
            foreach (var product in products)
            {
                productModels.Add(new ProductModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Sku = product.Sku,
                    StockQuantity = product.StockQuantity
                });
            }

            return productModels;
        }

        #endregion

        #region List

        public virtual async Task<IActionResult> List()
        {
            var model = new PurchaseOrderListModel();
            await PrepareSupplierListModelAsync(model);

            return View(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> PurchaseOrderList(PurchaseOrderListModel searchModel)
        {
            // Get purchase orders
            var purchaseOrders = await _purchaseOrderService.GetAllPurchaseOrdersAsync(
                supplierId: searchModel.SearchSupplierId,
                createdFromUtc: searchModel.SearchStartDate,
                createdToUtc: searchModel.SearchEndDate,
                pageIndex: searchModel.Page - 1,
                pageSize: searchModel.PageSize);

            // Prepare the data for the grid - use the appropriate model types
            var model = new PurchaseOrderListGridModel
            {
                Data = await purchaseOrders.SelectAwait(async po =>
                {
                    var poModel = po.ToModel<PurchaseOrderModel>();

                    // Get supplier name
                    var supplier = await _supplierService.GetSupplierByIdAsync(po.SupplierId);
                    poModel.SupplierName = supplier?.Name ?? string.Empty;

                    // Format status
                    poModel.PurchaseOrderStatus = ((PurchaseOrderStatus)po.PurchaseOrderStatusId).ToString();

                    // Format date
                    poModel.CreatedOn = (await _dateTimeHelper.ConvertToUserTimeAsync(po.CreatedOnUtc, DateTimeKind.Utc)).ToString();

                    // Format total amount
                    poModel.FormattedTotalAmount = $"{po.TotalAmount:C}";

                    // Get created by customer name
                    var customer = await _customerService.GetCustomerByIdAsync(po.CreatedByCustomerId);
                    poModel.CreatedByCustomerName = customer != null
                        ? (await _customerService.IsGuestAsync(customer)
                            ? await _localizationService.GetResourceAsync("Admin.Customers.Guest")
                            : customer.Email)
                        : string.Empty;

                    return poModel;
                }).ToListAsync(),
                RecordsTotal = purchaseOrders.TotalCount,
                RecordsFiltered = purchaseOrders.TotalCount
            };

            return Json(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> GetProductsBySupplier(int supplierId)
        {
            var products = await GetProductsBySupplierIdAsync(supplierId);
            return Json(products);
        }

        #endregion

        #region Create / Edit / Delete

        public virtual async Task<IActionResult> Create()
        {

            var model = new PurchaseOrderModel
            {
                CreatedOnUtc = DateTime.UtcNow
            };

            await PrepareSupplierListAsync(model);

            return View(model);
        }


        [HttpPost]
        public virtual async Task<IActionResult> Create(PurchaseOrderModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var purchaseOrder = new Domain.PurchaseOrder
                    {
                        SupplierId = model.SupplierId,
                        PurchaseOrderNumber = model.PurchaseOrderNumber,
                        CreatedOnUtc = DateTime.UtcNow,
                        PurchaseOrderStatusId = (int)PurchaseOrderStatus.Created,
                        CreatedByCustomerId = (await _workContext.GetCurrentCustomerAsync()).Id,
                        TotalAmount = model.PurchaseOrderItems?.Sum(x => x.TotalCost) ?? 0
                    };

                    await _purchaseOrderService.InsertPurchaseOrderAsync(purchaseOrder);

                    // Add purchase order items
                    if (model.PurchaseOrderItems != null && model.PurchaseOrderItems.Any())
                    {
                        foreach (var itemModel in model.PurchaseOrderItems)
                        {
                            var purchaseOrderItem = new PurchaseOrderItem
                            {
                                PurchaseOrderId = purchaseOrder.Id,
                                ProductId = itemModel.ProductId,
                                Quantity = itemModel.Quantity,
                                UnitCost = itemModel.UnitCost,
                                TotalCost = itemModel.TotalCost
                            };

                            await _purchaseOrderService.InsertPurchaseOrderItemAsync(purchaseOrderItem);
                        }
                    }

                    _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Plugin.Payments.PurchaseOrder.Admin.Added"));

                    return RedirectToAction("List");
                }
                catch (Exception ex)
                {
                    await _logger.ErrorAsync(ex.Message, ex);
                    _notificationService.ErrorNotification(ex.Message);
                }
            }

            // If we got this far, something failed, redisplay form
            await PrepareSupplierListAsync(model);

            return View(model);
        }

        public virtual async Task<IActionResult> Edit(int id)
        {

            // Try to get the purchase order with the specified id
            var purchaseOrder = await _purchaseOrderService.GetPurchaseOrderByIdAsync(id);
            if (purchaseOrder == null)
                return RedirectToAction("List");

            // Map entity to model
            var model = purchaseOrder.ToModel<PurchaseOrderModel>();

            // Get supplier name
            var supplier = await _supplierService.GetSupplierByIdAsync(purchaseOrder.SupplierId);
            model.SupplierName = supplier?.Name ?? string.Empty;

            // Get purchase order items
            var purchaseOrderItems = await _purchaseOrderService.GetPurchaseOrderItemsByPurchaseOrderIdAsync(purchaseOrder.Id);
            foreach (var item in purchaseOrderItems)
            {
                var product = await _productService.GetProductByIdAsync(item.ProductId);
                var itemModel = item.ToModel<PurchaseOrderItemModel>();

                if (product != null)
                {
                    itemModel.ProductName = product.Name;
                    itemModel.Sku = product.Sku;
                    itemModel.CurrentStock = product.StockQuantity;
                }

                itemModel.FormattedUnitCost = $"{item.UnitCost:C}";
                itemModel.FormattedTotalCost = $"{item.TotalCost:C}";

                model.PurchaseOrderItems.Add(itemModel);
            }

            await PrepareSupplierListAsync(model);

            // Prepare statuses
            model.AvailableStatuses = Enum.GetValues(typeof(PurchaseOrderStatus))
                .Cast<PurchaseOrderStatus>()
                .Select(s => new SelectListItem
                {
                    Text = s.ToString(),
                    Value = ((int)s).ToString(),
                    Selected = (int)s == purchaseOrder.PurchaseOrderStatusId
                }).ToList();

            return View(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> Edit(PurchaseOrderModel model)
        {

            // Try to get the purchase order with the specified id
            var purchaseOrder = await _purchaseOrderService.GetPurchaseOrderByIdAsync(model.Id);
            if (purchaseOrder == null)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                try
                {
                    var originalStatus = purchaseOrder.PurchaseOrderStatusId;

                    // We don't allow changing the supplier, as it would require recalculating all product associations
                    purchaseOrder.PurchaseOrderNumber = model.PurchaseOrderNumber;
                    purchaseOrder.PurchaseOrderStatusId = model.PurchaseOrderStatusId;

                    // Calculate total amount
                    purchaseOrder.TotalAmount = model.PurchaseOrderItems?.Sum(x => x.TotalCost) ?? 0;

                    await _purchaseOrderService.UpdatePurchaseOrderAsync(purchaseOrder);

                    // Update purchase order items
                    // First, delete existing items
                    var existingItems = await _purchaseOrderService.GetPurchaseOrderItemsByPurchaseOrderIdAsync(purchaseOrder.Id);
                    foreach (var existingItem in existingItems)
                    {
                        await _purchaseOrderService.DeletePurchaseOrderItemAsync(existingItem);
                    }

                    // Add new items
                    if (model.PurchaseOrderItems != null && model.PurchaseOrderItems.Any())
                    {
                        foreach (var itemModel in model.PurchaseOrderItems)
                        {
                            var purchaseOrderItem = new PurchaseOrderItem
                            {
                                PurchaseOrderId = purchaseOrder.Id,
                                ProductId = itemModel.ProductId,
                                Quantity = itemModel.Quantity,
                                UnitCost = itemModel.UnitCost,
                                TotalCost = itemModel.TotalCost
                            };

                            await _purchaseOrderService.InsertPurchaseOrderItemAsync(purchaseOrderItem);
                        }
                    }

                    // Update inventory if status is changed to Received
                    if (originalStatus != (int)PurchaseOrderStatus.Received &&
                        purchaseOrder.PurchaseOrderStatusId == (int)PurchaseOrderStatus.Received)
                    {
                        await _purchaseOrderService.UpdateInventoryAsync(purchaseOrder);
                    }

                    _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Plugin.Payments.PurchaseOrder.Admin.Updated"));

                    return RedirectToAction("List");
                }
                catch (Exception ex)
                {
                    await _logger.ErrorAsync(ex.Message, ex);
                    _notificationService.ErrorNotification(ex.Message);
                }
            }

            // If we got this far, something failed, redisplay form
            // Get supplier name
            var supplier = await _supplierService.GetSupplierByIdAsync(purchaseOrder.SupplierId);
            model.SupplierName = supplier?.Name ?? string.Empty;

            // Reload items
            model.PurchaseOrderItems.Clear();
            var purchaseOrderItems = await _purchaseOrderService.GetPurchaseOrderItemsByPurchaseOrderIdAsync(purchaseOrder.Id);
            foreach (var item in purchaseOrderItems)
            {
                var product = await _productService.GetProductByIdAsync(item.ProductId);
                var itemModel = item.ToModel<PurchaseOrderItemModel>();

                if (product != null)
                {
                    itemModel.ProductName = product.Name;
                    itemModel.Sku = product.Sku;
                    itemModel.CurrentStock = product.StockQuantity;
                }

                itemModel.FormattedUnitCost = $"{item.UnitCost:C}";
                itemModel.FormattedTotalCost = $"{item.TotalCost:C}";

                model.PurchaseOrderItems.Add(itemModel);
            }

            await PrepareSupplierListAsync(model);

            // Prepare statuses

            // Fix for CA2263: Use the generic overload of Enum.GetValues<TEnum>().
            model.AvailableStatuses = Enum.GetValues<PurchaseOrderStatus>()
                .Select(s => new SelectListItem
                {
                    Text = s.ToString(),
                    Value = ((int)s).ToString(),
                    Selected = (int)s == purchaseOrder.PurchaseOrderStatusId
                }).ToList();

            return View(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> Delete(int id)
        {

            // Try to get the purchase order with the specified id
            var purchaseOrder = await _purchaseOrderService.GetPurchaseOrderByIdAsync(id);
            if (purchaseOrder == null)
                return RedirectToAction("List");

            try
            {
                await _purchaseOrderService.DeletePurchaseOrderAsync(purchaseOrder);
                _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Plugin.Payments.PurchaseOrder.Admin.Deleted"));
            }
            catch (Exception ex)
            {
                await _logger.ErrorAsync(ex.Message, ex);
                _notificationService.ErrorNotification(ex.Message);
            }

            return RedirectToAction("List");
        }

        #endregion

        #region Inventory Management

        [HttpPost]
        public virtual async Task<IActionResult> UpdateInventory(int id)
        { 
            var purchaseOrder = await _purchaseOrderService.GetPurchaseOrderByIdAsync(id);
            if (purchaseOrder == null)
                return RedirectToAction("List");

            try
            {
                // Update inventory based on purchase order
                await _purchaseOrderService.UpdateInventoryAsync(purchaseOrder);

                // Update purchase order status to Received
                purchaseOrder.PurchaseOrderStatusId = (int)PurchaseOrderStatus.Received;
                await _purchaseOrderService.UpdatePurchaseOrderAsync(purchaseOrder);

                _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Plugin.Misc.PurchaseOrder.Admin.InventoryUpdated"));
            }
            catch (Exception ex)
            {
                await _logger.ErrorAsync(ex.Message, ex);
                _notificationService.ErrorNotification(ex.Message);
            }

            return RedirectToAction("Edit", new { id = purchaseOrder.Id });
        }

        #endregion
    }


}