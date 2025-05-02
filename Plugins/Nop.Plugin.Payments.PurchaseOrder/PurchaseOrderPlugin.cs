using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;
using Nop.Core;
using Nop.Core.Configuration;
using Nop.Core.Infrastructure;
using Nop.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Nop.Core.Domain.Cms;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Payments;
using Nop.Plugin.Payments.PurchaseOrder.Areas.Admin.Infrastructure.Mapping;
using Nop.Plugin.Payments.PurchaseOrder.Areas.Admin.Domain;
using Nop.Plugin.Payments.PurchaseOrder.Areas.Admin.Services;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Payments;
using Nop.Services.Plugins;
using Nop.Services.Stores;
using Nop.Web.Framework.Infrastructure;
using Nop.Web.Framework.Menu;
using Nop.Services.Security;


namespace Nop.Plugin.Payments.PurchaseOrder
{
    /// <summary>
    /// Represents the purchase order plugin
    /// </summary>
    public class PurchaseOrderPlugin : BasePlugin
    {
        private readonly IPermissionService _permissionService;
        private readonly ILocalizationService _localizationService;
        private readonly INopDataProvider _dataProvider;
        private readonly IWebHelper _webHelper;

        public PurchaseOrderPlugin(
            IPermissionService permissionService,
            ILocalizationService localizationService,
            INopDataProvider dataProvider,
            IWebHelper webHelper)
        {
            _permissionService = permissionService;
            _localizationService = localizationService;
            _dataProvider = dataProvider;
            _webHelper = webHelper;
        }

        public override async Task InstallAsync()
        {
            // Add localization resources
            

            

            await _dataProvider.ExecuteNonQueryAsync("CREATE TABLE IF NOT EXISTS [PurchaseOrder] (...)");

            await _dataProvider.ExecuteNonQueryAsync("CREATE TABLE IF NOT EXISTS [PurchaseOrderItem] (...)");

            await base.InstallAsync();
        }

        public override async Task UninstallAsync()
        {
           

           

            await _dataProvider.ExecuteNonQueryAsync("DROP TABLE IF EXISTS [PurchaseOrder]");

            await _dataProvider.ExecuteNonQueryAsync("DROP TABLE IF EXISTS [PurchaseOrderItem]");

            await base.UninstallAsync();
        }

        public override async Task UpdateAsync(string currentVersion, string targetVersion)
        {
            var current = Version.Parse(currentVersion);
            var target = Version.Parse(targetVersion);

            if (current < target)
            {
                await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
                {
                    ["Plugin.Payments.PurchaseOrder.Admin.Added"] = "The purchase order has been added successfully.",
                    ["Plugin.Payments.PurchaseOrder.Admin.Updated"] = "The purchase order has been updated successfully.",
                    ["Plugin.Payments.PurchaseOrder.Admin.Deleted"] = "The purchase order has been deleted successfully.",
                    ["Plugin.Payments.PurchaseOrder.Admin.Menu.Title"] = "Purchase Orders",
                    ["Plugin.Payments.PurchaseOrder.List.SearchSupplier"] = "Supplier",
                    ["Plugin.Payments.PurchaseOrder.List.SearchStartDate"] = "Start date",
                    ["Plugin.Payments.PurchaseOrder.List.SearchEndDate"] = "End date",
                    ["Plugin.Payments.PurchaseOrder.Fields.Supplier"] = "Supplier",
                    ["Plugin.Payments.PurchaseOrder.Fields.PurchaseOrderNumber"] = "Purchase Order #",
                    ["Plugin.Payments.PurchaseOrder.Fields.CreatedOn"] = "Created on",
                    ["Plugin.Payments.PurchaseOrder.Fields.Status"] = "Status",
                    ["Plugin.Payments.PurchaseOrder.Fields.CreatedBy"] = "Created by",
                    ["Plugin.Payments.PurchaseOrder.Fields.TotalAmount"] = "Total amount",
                    ["Plugin.Payments.PurchaseOrder.PurchaseOrderItem.Fields.Product"] = "Product",
                    ["Plugin.Payments.PurchaseOrder.PurchaseOrderItem.Fields.Sku"] = "SKU",
                    ["Plugin.Payments.PurchaseOrder.PurchaseOrderItem.Fields.CurrentStock"] = "Current stock",
                    ["Plugin.Payments.PurchaseOrder.PurchaseOrderItem.Fields.Quantity"] = "Quantity",
                    ["Plugin.Payments.PurchaseOrder.PurchaseOrderItem.Fields.UnitCost"] = "Unit cost",
                    ["Plugin.Payments.PurchaseOrder.PurchaseOrderItem.Fields.TotalCost"] = "Total cost",
                    ["Plugin.Payments.PurchaseOrder.Create"] = "Create purchase order",
                    ["Plugin.Payments.PurchaseOrder.Edit"] = "Edit purchase order",
                    ["Plugin.Payments.PurchaseOrder.List"] = "Purchase orders",
                    ["Plugin.Payments.PurchaseOrder.AddProduct"] = "Add product",
                    ["Plugin.Payments.PurchaseOrder.SelectSupplierFirst"] = "Please select a supplier first."

                });

            }
        }

    }
}