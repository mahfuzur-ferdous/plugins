using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Misc.Suppliers.Areas.Admin.Factories;
using Nop.Plugin.Misc.Suppliers.Areas.Admin.Models;
using Nop.Plugin.Misc.Suppliers.Areas.Admin.Services;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Media;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Services.Seo;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Factories;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Misc.Suppliers.Areas.Admin.Controllers
{
    [AuthorizeAdmin]
    [Area("Admin")]
    [AutoValidateAntiforgeryToken]
    public class SupplierAdminController : BasePluginController
    {
        #region Fields
        protected readonly ISupplierService _supplierService;
        protected readonly ICustomerActivityService _customerActivityService;
        protected readonly ILocalizationService _localizationService;
        protected readonly INotificationService _notificationService;
        protected readonly IPermissionService _permissionService;
        protected readonly IPictureService _pictureService;
        protected readonly IUrlRecordService _urlRecordService;
        protected readonly ILocalizedEntityService _localizedEntityService;
        protected readonly ISupplierModelFactory _supplierModelFactory;
        protected readonly ILocalizedModelFactory _localizedModelFactory;
        #endregion

        #region Ctor
        public SupplierAdminController(
            ISupplierService supplierService,
            ICustomerActivityService customerActivityService,
            ILocalizationService localizationService,
            INotificationService notificationService,
            IPermissionService permissionService,
            IPictureService pictureService,
            IUrlRecordService urlRecordService,
            ILocalizedEntityService localizedEntityService,
            ISupplierModelFactory supplierModelFactory,
            ILocalizedModelFactory localizedModelFactory)
        {
            _supplierService = supplierService;
            _customerActivityService = customerActivityService;
            _localizationService = localizationService;
            _notificationService = notificationService;
            _permissionService = permissionService;
            _pictureService = pictureService;
            _urlRecordService = urlRecordService;
            _localizedEntityService = localizedEntityService;
            _supplierModelFactory = supplierModelFactory;
            _localizedModelFactory = localizedModelFactory;
        }
        #endregion

        #region Utilities
        protected virtual async Task UpdateLocalesAsync(Domain.Supplier supplier, SupplierModel model)
        {
            foreach (var localized in model.Locales)
            {
                await _localizedEntityService.SaveLocalizedValueAsync(supplier,
                    x => x.Name,
                    localized.Name,
                    localized.LanguageId);

                await _localizedEntityService.SaveLocalizedValueAsync(supplier,
                    x => x.Description,
                    localized.Description,
                    localized.LanguageId);
            }
        }
        #endregion

        #region Methods
        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual async Task<IActionResult> List()
        {
            var model = await _supplierModelFactory.PrepareSupplierSearchModelAsync(new SupplierSearchModel());
            return View("~/Plugins/Misc.Suppliers/Areas/Admin/Views/SupplierAdmin/List.cshtml", model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> List(SupplierSearchModel searchModel)
        {
            var model = await _supplierModelFactory.PrepareSupplierListModelAsync(searchModel);
            return Json(model);
        }

        public virtual async Task<IActionResult> Create()
        {
            var model = await _supplierModelFactory.PrepareSupplierModelAsync(new SupplierModel(), null);

            model.Locales = await _localizedModelFactory.PrepareLocalizedModelsAsync<SupplierLocalizedModel>(
                async (locale, languageId) =>
                {
                    locale.LanguageId = languageId;
                    await Task.CompletedTask;
                });

            return View("~/Plugins/Misc.Suppliers/Areas/Admin/Views/SupplierAdmin/Create.cshtml", model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public virtual async Task<IActionResult> Create(SupplierModel model, bool continueEditing, IFormCollection form)
        {
            if (ModelState.IsValid)
            {
                var supplier = new Domain.Supplier
                {
                    Name = model.Name,
                    Email = model.Email,
                    Description = model.Description,
                    Active = model.Active,
                    CreatedOnUtc = DateTime.UtcNow
                };

                await _supplierService.InsertSupplierAsync(supplier);

                await UpdateLocalesAsync(supplier, model);

                await _customerActivityService.InsertActivityAsync("AddNewSupplier",
                    string.Format(await _localizationService.GetResourceAsync("ActivityLog.AddNewSupplier"), supplier.Id), supplier);

                _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Plugins.Misc.Suppliers.Added"));

                if (!continueEditing)
                    return RedirectToAction("List");

                return RedirectToAction("Edit", new { id = supplier.Id });
            }
            model = await _supplierModelFactory.PrepareSupplierModelAsync(model, null);
            return View("~/Plugins/Misc.Suppliers/Areas/Admin/Views/SupplierAdmin/Create.cshtml", model);
        }

        public virtual async Task<IActionResult> Edit(int id)
        {
            var supplier = await _supplierService.GetSupplierByIdAsync(id);
            if (supplier == null)
                return RedirectToAction("List");

            var model = await _supplierModelFactory.PrepareSupplierModelAsync(null, supplier);

            model.Locales = await _localizedModelFactory.PrepareLocalizedModelsAsync<SupplierLocalizedModel>(
                async (locale, languageId) =>
                {
                    locale.LanguageId = languageId;
                    locale.Name = await _localizationService.GetLocalizedAsync(supplier, entity => entity.Name, languageId, false, false);
                    locale.Description = await _localizationService.GetLocalizedAsync(supplier, entity => entity.Description, languageId, false, false);
                });

            return View("~/Plugins/Misc.Suppliers/Areas/Admin/Views/SupplierAdmin/Edit.cshtml", model);
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

                await UpdateLocalesAsync(supplier, model);

                _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Plugins.Misc.Suppliers.Updated"));

                if (continueEditing)
                    return RedirectToAction("Edit", new { id = supplier.Id });

                return RedirectToAction("List");
            }

            model = await _supplierModelFactory.PrepareSupplierModelAsync(model, supplier);
            return View("~/Plugins/Misc.Suppliers/Areas/Admin/Views/SupplierAdmin/Edit.cshtml", model);
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

        [HttpPost]
        public async Task<IActionResult> AssignSupplierToProduct(int productId, int supplierId)
        {
            if (productId == 0 || supplierId == 0)
                return Json(new { success = false, message = "Invalid product or supplier ID" });

            await _supplierService.InsertOrUpdateProductSupplierMappingAsync(productId, supplierId);

            return Json(new { success = true, message = "Supplier added to product successfully." });
        }

        public static string StripPTags(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;
            return Regex.Replace(input, @"^<p>(.*?)</p>$", "$1", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        }
        #endregion
    }
}