// Nop.Plugin.Misc.Suppliers/SuppliersPlugin.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Plugins;

namespace Nop.Plugin.Misc.Suppliers
{
    /// <summary>
    /// Represents the suppliers plugin
    /// </summary>
    public class SuppliersPlugin : BasePlugin, IMiscPlugin
    {
        #region Fields

        private readonly ILocalizationService _localizationService;
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;

        #endregion

        #region Ctor

        public SuppliersPlugin(
            ILocalizationService localizationService,
            ISettingService settingService,
            IWebHelper webHelper)
        {
            _localizationService = localizationService;
            _settingService = settingService;
            _webHelper = webHelper;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a configuration page URL
        /// </summary>
        public override string GetConfigurationPageUrl()
        {
            return $"{_webHelper.GetStoreLocation()}Admin/SupplierAdmin/List";
        }

        /// <summary>
        /// Install the plugin
        /// </summary>
        public override async Task InstallAsync()
        {
            //locales
            await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
            {
                ["Plugins.Misc.Suppliers.Fields.Name"] = "Name",
                ["Plugins.Misc.Suppliers.Fields.Email"] = "Email",
                ["Plugins.Misc.Suppliers.Fields.Description"] = "Description",
                ["Plugins.Misc.Suppliers.Fields.Active"] = "Active",
                ["Plugins.Misc.Suppliers.Fields.CreatedOn"] = "Created on",
                ["Plugins.Misc.Suppliers.Added"] = "The supplier has been added successfully.",
                ["Plugins.Misc.Suppliers.Updated"] = "The supplier has been updated successfully.",
                ["Plugins.Misc.Suppliers.Deleted"] = "The supplier has been deleted successfully.",
                ["Plugins.Misc.Suppliers.List.SearchName"] = "Supplier name",
                ["Plugins.Misc.Suppliers.List.SearchEmail"] = "Supplier email",
                ["Plugins.Misc.Suppliers.Menu.Suppliers"] = "Suppliers"
            });

            await base.InstallAsync();
        }

        /// <summary>
        /// Uninstall the plugin
        /// </summary>
        public override async Task UninstallAsync()
        {
            //locales
            await _localizationService.DeleteLocaleResourcesAsync("Plugins.Misc.Suppliers");

            await base.UninstallAsync();
        }

        #endregion
    }
}