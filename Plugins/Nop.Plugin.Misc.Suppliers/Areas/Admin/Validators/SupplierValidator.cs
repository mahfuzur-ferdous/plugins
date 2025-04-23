using FluentValidation;
using Nop.Plugin.Misc.Suppliers.Areas.Admin.Models;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;

namespace Nop.Plugin.Misc.Suppliers.Areas.Admin.Validators;

public partial class SupplierValidator : BaseNopValidator<SupplierModel>
{
    public SupplierValidator(ILocalizationService localizationService)
    {
        RuleFor(x => x.Name).NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("Admin.Suppliers.Fields.Name.Required"));

        RuleFor(x => x.Email).NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("Admin.Suppliers.Fields.Email.Required"));
        RuleFor(x => x.Email)
            .IsEmailAddress()
            .WithMessageAwait(localizationService.GetResourceAsync("Admin.Common.WrongEmail"));
    }
}