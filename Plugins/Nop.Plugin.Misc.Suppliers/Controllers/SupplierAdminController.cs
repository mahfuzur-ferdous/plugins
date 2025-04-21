using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Misc.Suppliers.Domain;
using Nop.Plugin.Misc.Suppliers.Models;
using Nop.Plugin.Misc.Suppliers.Services;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Models.Extensions;
using Nop.Web.Framework.Mvc.Filters;


namespace Nop.Plugin.Misc.Suppliers.Controllers
{
    [AuthorizeAdmin]
    [Area("admin")]
    [AutoValidateAntiforgeryToken]
    public class SupplierAdminController : BasePluginController
    {
        #region Fields

        private readonly ILocalizationService _localizationService;
        private readonly INotificationService _notificationService;
        private readonly IPermissionService _permissionService;
        private readonly ISupplierService _supplierService;

        // Define a constant for supplier permission
        private const string MANAGE_SUPPLIERS_PERMISSION = "ManageSuppliers";

        #endregion

        #region Ctor

        public SupplierAdminController(
            ILocalizationService localizationService,
            INotificationService notificationService,
            IPermissionService permissionService,
            ISupplierService supplierService)
        {
            _localizationService = localizationService;
            _notificationService = notificationService;
            _permissionService = permissionService;
            _supplierService = supplierService;
        }

        #endregion

        #region Methods

        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual async Task<IActionResult> List()
        {
            

            var model = new SupplierSearchModel();
            return View("~/Plugins/Misc.Suppliers/Views/SupplierAdmin/List.cshtml", model);
        }

        [HttpPost]
        public virtual async Task<SupplierListModel> List(SupplierSearchModel searchModel)
        {
           

            //get suppliers
            var suppliers = await _supplierService.GetAllSuppliersAsync(
                searchModel.SearchName,
                searchModel.SearchEmail,
                searchModel.Page - 1,
                searchModel.PageSize);

            //prepare list model
            var model = new SupplierListModel().PrepareToGrid(searchModel, suppliers, () =>
            {
                return suppliers.Select(supplier =>
                {
                    var supplierModel = new SupplierModel
                    {
                        Id = supplier.Id,
                        Name = supplier.Name,
                        Email = supplier.Email,
                        Active = supplier.Active,
                        CreatedOn = supplier.CreatedOnUtc
                    };

                    return supplierModel;
                });
            });

            return model;
        }

        public virtual async Task<IActionResult> Create()
        {
            

            var model = new SupplierModel();
            return View("~/Plugins/Misc.Suppliers/Views/SupplierAdmin/Create.cshtml", model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual async Task<IActionResult> Create(SupplierModel model, bool continueEditing)
        {
            

            if (ModelState.IsValid)
            {
                var supplier = new Domain.Supplier
                {
                    Name = model.Name,
                    Email = model.Email,
                    Description = model.Description,
                    Active = model.Active
                };

                await _supplierService.InsertSupplierAsync(supplier);

                _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Plugins.Misc.Suppliers.Added"));

                if (continueEditing)
                    return RedirectToAction("Edit", new { id = supplier.Id });

                return RedirectToAction("List");
            }

            return View("~/Plugins/Misc.Suppliers/Views/SupplierAdmin/Create.cshtml", model);
        }

        public virtual async Task<IActionResult> Edit(int id)
        {
            

            var supplier = await _supplierService.GetSupplierByIdAsync(id);
            if (supplier == null)
                return RedirectToAction("List");

            var model = new SupplierModel
            {
                Id = supplier.Id,
                Name = supplier.Name,
                Email = supplier.Email,
                Description = supplier.Description,
                Active = supplier.Active,
                CreatedOn = supplier.CreatedOnUtc
            };

            return View("~/Plugins/Misc.Suppliers/Views/SupplierAdmin/Edit.cshtml", model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual async Task<IActionResult> Edit(SupplierModel model, bool continueEditing)
        {
            

            var supplier = await _supplierService.GetSupplierByIdAsync(model.Id);
            if (supplier == null)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                supplier.Name = model.Name;
                supplier.Email = model.Email;
                supplier.Description = model.Description;
                supplier.Active = model.Active;

                await _supplierService.UpdateSupplierAsync(supplier);

                _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Plugins.Misc.Suppliers.Updated"));

                if (continueEditing)
                    return RedirectToAction("Edit", new { id = supplier.Id });

                return RedirectToAction("List");
            }

            return View("~/Plugins/Misc.Suppliers/Views/SupplierAdmin/Edit.cshtml", model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> Delete(int id)
        {
            

            var supplier = await _supplierService.GetSupplierByIdAsync(id);
            if (supplier == null)
                return RedirectToAction("List");

            await _supplierService.DeleteSupplierAsync(supplier);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Plugins.Misc.Suppliers.Deleted"));

            return RedirectToAction("List");
        }

        #endregion
    }
}