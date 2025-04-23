using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.Suppliers.Areas.Admin.Models;
public record SupplierLocalizedModel : BaseNopEntityModel, ILocalizedLocaleModel
{
    public int LanguageId { get; set; }

    [NopResourceDisplayName("Admin.Suppliers.Fields.Name")]
    public string Name { get; set; }

    [NopResourceDisplayName("Admin.Suppliers.Fields.Description")]
    public string Description { get; set; }
}
