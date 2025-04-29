using System;
using Nop.Core.Caching;
using Nop.Plugin.Misc.Suppliers.Areas.Admin.Models;
using Nop.Plugin.Misc.Suppliers.Areas.Admin.Services;
using Nop.Plugin.Misc.Suppliers.Areas.Admin.Domain;
using Nop.Services.Customers;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Seo;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Framework.Factories;
using Nop.Web.Framework.Models.Extensions;
using System.Collections.Generic;
using Nop.Core.Domain.Localization;

namespace Nop.Plugin.Misc.Suppliers.Areas.Admin.Factories
{
    /// <summary>
    /// Represents the supplier model factory
    /// </summary>
    public partial class SupplierModelFactory : ISupplierModelFactory
    {
        #region Fields

        private readonly IBaseAdminModelFactory _baseAdminModelFactory;
        private readonly ICustomerService _customerService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedModelFactory _localizedModelFactory;
        private readonly IPictureService _pictureService;
        private readonly ISupplierService _supplierService;
        private readonly IUrlRecordService _urlRecordService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly ILanguageService _languageService;

        #endregion

        #region Ctor

        public SupplierModelFactory(
            IBaseAdminModelFactory baseAdminModelFactory,
            ICustomerService customerService,
            IDateTimeHelper dateTimeHelper,
            ILocalizationService localizationService,
            ILocalizedModelFactory localizedModelFactory,
            IPictureService pictureService,
            ISupplierService supplierService,
            IUrlRecordService urlRecordService,
            ILocalizedEntityService localizedEntityService,
            IStaticCacheManager staticCacheManager,
            ILanguageService languageService) // <-- added here
        {
            _baseAdminModelFactory = baseAdminModelFactory;
            _customerService = customerService;
            _dateTimeHelper = dateTimeHelper;
            _localizationService = localizationService;
            _localizedModelFactory = localizedModelFactory;
            _pictureService = pictureService;
            _supplierService = supplierService;
            _urlRecordService = urlRecordService;
            _localizedEntityService = localizedEntityService;
            _staticCacheManager = staticCacheManager;
            _languageService = languageService; // <-- assigned here
        }

        #endregion

        #region Methods

        public virtual async Task<SupplierSearchModel> PrepareSupplierSearchModelAsync(SupplierSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            searchModel.SetGridPageSize();

            return searchModel;
        }

        public virtual async Task<SupplierListModel> PrepareSupplierListModelAsync(SupplierSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            var suppliers = await _supplierService.GetAllSuppliersAsync(
                name: searchModel.SearchName,
                email: searchModel.SearchEmail,
                pageIndex: searchModel.Page - 1,
                pageSize: searchModel.PageSize);

            var model = await new SupplierListModel().PrepareToGridAsync(searchModel, suppliers, () =>
            {
                return suppliers.SelectAwait(async supplier =>
                {
                    var supplierModel = new SupplierModel
                    {
                        Id = supplier.Id,
                        Name = supplier.Name,
                        Email = supplier.Email,
                        Active = supplier.Active,
                        CreatedOn = await _dateTimeHelper.ConvertToUserTimeAsync(supplier.CreatedOnUtc, DateTimeKind.Utc)
                    };

                    return supplierModel;
                });
            });

            return model;
        }

        public virtual async Task<SupplierModel> PrepareSupplierModelAsync(SupplierModel model, Domain.Supplier supplier, bool excludeProperties = false)
        {
            if (supplier != null)
            {
                if (model == null)
                {
                    model = new SupplierModel
                    {
                        Id = supplier.Id,
                        Name = supplier.Name,
                        Email = supplier.Email,
                        Description = supplier.Description,
                        Active = supplier.Active,
                        CreatedOn = await _dateTimeHelper.ConvertToUserTimeAsync(supplier.CreatedOnUtc, DateTimeKind.Utc)
                    };
                }

                if (!excludeProperties)
                    await PrepareSupplierLocalizedModelsAsync(model, supplier);
            }

            return model ?? new SupplierModel();
        }

        protected virtual async Task PrepareSupplierLocalizedModelsAsync(SupplierModel model, Domain.Supplier supplier)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (supplier == null)
                throw new ArgumentNullException(nameof(supplier));

            model.Locales = new List<SupplierLocalizedModel>();


            var languages = await _languageService.GetAllLanguagesAsync(showHidden: true);

            foreach (var language in languages)
            {
                var locale = new SupplierLocalizedModel
                {
                    LanguageId = language.Id,
                    Name = await _localizationService.GetLocalizedAsync(supplier, entity => entity.Name, language.Id, false, false),
                    Description = await _localizationService.GetLocalizedAsync(supplier, entity => entity.Description, language.Id, false, false)
                };

                model.Locales.Add(locale);
            }
        }

        public virtual async Task<SupplierListModel> PrepareExtendedSupplierListModelAsync(SupplierSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(
                SupplierDefaults.AdminSupplierAllModelKey,
                searchModel.SearchName, searchModel.SearchEmail, searchModel.Page - 1, searchModel.PageSize);

            var model = await _staticCacheManager.GetAsync<SupplierListModel>(cacheKey);
            if (model != null)
                return model;

            var suppliers = await _supplierService.GetAllSuppliersAsync(
                name: searchModel.SearchName,
                email: searchModel.SearchEmail,
                pageIndex: searchModel.Page - 1,
                pageSize: searchModel.PageSize);

            model = await new SupplierListModel().PrepareToGridAsync(searchModel, suppliers, () =>
            {
                return suppliers.SelectAwait(async supplier =>
                {
                    var supplierModel = new SupplierModel
                    {
                        Id = supplier.Id,
                        Name = supplier.Name,
                        Email = supplier.Email,
                        Description = supplier.Description,
                        Active = supplier.Active,
                        CreatedOn = await _dateTimeHelper.ConvertToUserTimeAsync(supplier.CreatedOnUtc, DateTimeKind.Utc)
                    };

                    return supplierModel;
                });
            });

            await _staticCacheManager.SetAsync(cacheKey, model);

            return model;
        }

        public virtual async Task InsertOrUpdateProductSupplierAsync(int productId, int supplierId)
        {
            if (productId <= 0)
                throw new ArgumentException("Product ID must be greater than zero", nameof(productId));

            if (supplierId <= 0)
                throw new ArgumentException("Supplier ID must be greater than zero", nameof(supplierId));

            await _supplierService.InsertOrUpdateProductSupplierMappingAsync(productId, supplierId);
        }

        public virtual async Task<int> GetProductSupplierIdAsync(int productId)
        {
            if (productId <= 0)
                return 0;

            return await _supplierService.GetProductSupplierIdAsync(productId);
        }

        #endregion
    }
}
