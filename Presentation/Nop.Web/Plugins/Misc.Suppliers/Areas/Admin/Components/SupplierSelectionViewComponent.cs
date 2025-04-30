using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Misc.Suppliers.Areas.Admin.Models;
using Nop.Plugin.Misc.Suppliers.Areas.Admin.Services;
using Nop.Web.Areas.Admin.Models.Catalog;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Misc.Suppliers.Areas.Admin.Components;
public class SupplierSelectionViewComponent : NopViewComponent
{
    private readonly ISupplierService _supplierService;

    public SupplierSelectionViewComponent(ISupplierService supplierService)
    {
        _supplierService = supplierService;
    }

    public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
    {
        var productModel = additionalData as ProductModel;

        if (productModel == null || productModel.Id == 0)
            return View("Create");

        var suppliers = await _supplierService.GetAllSuppliersAsync();
        var supplierId = await _supplierService.GetProductSupplierIdAsync(productModel.Id);

        var model = new SupplierSelectionModel
        {
            ProductId = productModel.Id,
            SelectedSupplierId = supplierId,
            SelectedSupplierName = suppliers?.FirstOrDefault(s => s.Id == supplierId)?.Name,
            Suppliers = suppliers.Select(s => new SupplierModel
            {
                Id = s.Id,
                Name = s.Name,
                Email = s.Email,
                Description = s.Description,
                Active = s.Active
            }).ToList()
        };

        return View("Edit", model);
    }
}