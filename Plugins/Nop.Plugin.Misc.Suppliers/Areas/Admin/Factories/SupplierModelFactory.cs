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
            ILocalizedEntityService localizedEntityService)
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
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prepare supplier search model
        /// </summary>
        /// <param name="searchModel">Supplier search model</param>
        /// <returns>Supplier search model</returns>
        public virtual async Task<SupplierSearchModel> PrepareSupplierSearchModelAsync(SupplierSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            searchModel.SetGridPageSize();

            return searchModel;
        }

        /// <summary>
        /// Prepare paged supplier list model
        /// </summary>
        /// <param name="searchModel">Supplier search model</param>
        /// <returns>Supplier list model</returns>
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

        /// <summary>
        /// Prepare supplier model
        /// </summary>
        /// <param name="model">Supplier model</param>
        /// <param name="supplier">Supplier</param>
        /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
        /// <returns>Supplier model</returns>
        public virtual async Task<SupplierModel> PrepareSupplierModelAsync(SupplierModel model, Supplier supplier, bool excludeProperties = false)
        {
            if (supplier != null)
                if (model == null)
                    model = new SupplierModel
                    {
                        Id = supplier.Id,
                        Name = supplier.Name,
                        Email = supplier.Email,
                        Description = supplier.Description,
                        Active = supplier.Active,
                        CreatedOn = await _dateTimeHelper.ConvertToUserTimeAsync(supplier.CreatedOnUtc, DateTimeKind.Utc)
                    };
            Func<SupplierLocalizedModel, int, Task> localizedModelConfiguration = null;
            localizedModelConfiguration = async (locale, languageId) =>
            {
                locale.Name = await _localizationService.GetLocalizedAsync(supplier, entity => entity.Name, languageId, false, false);
                locale.Description = await _localizationService.GetLocalizedAsync(supplier, entity => entity.Description, languageId, false, false);
            };
            return model ?? new SupplierModel();
        }
    }
    #endregion
}
