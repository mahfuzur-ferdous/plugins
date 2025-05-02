using Nop.Core;
using Nop.Data;
using Nop.Plugin.Misc.Suppliers.Areas.Admin.Components;
using Nop.Plugin.Misc.Suppliers.Areas.Admin.Utilities;
using Nop.Services.Cms;
using Nop.Services.Common;
using Nop.Services.Localization;
using Nop.Services.Plugins;
using Nop.Services.Security;
using Nop.Web.Framework.Infrastructure;

namespace Nop.Plugin.Misc.Suppliers;

public class SupplierPlugin : BasePlugin, IMiscPlugin, IWidgetPlugin
{
    private readonly IPermissionService _permissionService;
    private readonly ILocalizationService _localizationService;
    private readonly INopDataProvider _dataProvider;
    private readonly IWebHelper _webHelper;

    public SupplierPlugin(
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
        var resources = SupplierLocaleResources.GetAll();

        await _localizationService.AddOrUpdateLocaleResourceAsync(resources);

        await _dataProvider.ExecuteNonQueryAsync("CREATE TABLE IF NOT EXISTS [Supplier] (...)");

        await _dataProvider.ExecuteNonQueryAsync("CREATE TABLE IF NOT EXISTS [ProductSupplierMapping] (...)");

        await base.InstallAsync();
    }

    public override async Task UninstallAsync()
    {
        var resourceKeys = SupplierLocaleResources.GetAll().Keys.ToArray();

        await _localizationService.DeleteLocaleResourcesAsync(resourceKeys);

        await _dataProvider.ExecuteNonQueryAsync("DROP TABLE IF EXISTS [Supplier]");

        await _dataProvider.ExecuteNonQueryAsync("DROP TABLE IF EXISTS [ProductSupplierMapping]");

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
                ["Admin.Suppliers.Fields.Email.Required"] = "Email is Required!"
            });

            // Add any schema updates if needed
        }
    }

    /// <summary>
    /// Gets a value indicating whether to hide this plugin on the widget list page in the admin area
    /// </summary>
    public bool HideInWidgetList => false;

    /// <summary>
    /// Gets widget zones where this widget should be rendered
    /// </summary>
    /// <returns>Widget zones</returns>
    public Task<IList<string>> GetWidgetZonesAsync()
    {
        return Task.FromResult<IList<string>>(new List<string>
        {
            AdminWidgetZones.ProductDetailsBlock
        });
    }

    /// <summary>
    /// Gets a type of a view component for displaying widget
    /// </summary>
    /// <param name="widgetZone">Name of the widget zone</param>
    /// <returns>View component type</returns>
    public Type GetWidgetViewComponent(string widgetZone)
    {
        return typeof(SupplierSelectionViewComponent);
    }
}