// Nop.Plugin.Misc.Suppliers/Models/SupplierModel.cs
using System;
using System.ComponentModel.DataAnnotations;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;


namespace Nop.Plugin.Misc.Suppliers.Models;

    /// <summary>
    /// Represents a supplier model
    /// </summary>
    public record SupplierModel : BaseNopEntityModel, ILocalizedModel<SupplierLocalizedModel>
    {
        #region Ctor

        #endregion

        #region Properties

        [NopResourceDisplayName("Plugins.Misc.Suppliers.Fields.Name")]
        public string Name { get; set; }

        [DataType(DataType.EmailAddress)]
        [NopResourceDisplayName("Plugins.Misc.Suppliers.Fields.Email")]
        public string Email { get; set; }

        [NopResourceDisplayName("Plugins.Misc.Suppliers.Fields.Description")]
        public string Description { get; set; }

        [NopResourceDisplayName("Plugins.Misc.Suppliers.Fields.Active")]
        public bool Active { get; set; } = true;

        [NopResourceDisplayName("Plugins.Misc.Suppliers.Fields.CreatedOn")]
        public DateTime CreatedOn { get; set; }

        public IList<SupplierLocalizedModel> Locales { get; set; }

        public SupplierModel()
        {
            Locales = new List<SupplierLocalizedModel>();
        }

        #endregion
    }
