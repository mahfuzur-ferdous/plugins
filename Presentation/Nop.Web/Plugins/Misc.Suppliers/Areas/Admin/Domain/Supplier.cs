using Nop.Core;
using Nop.Core.Domain.Localization;

namespace Nop.Plugin.Misc.Suppliers.Areas.Admin.Domain
{
    /// <summary>
    /// Represents a supplier
    /// </summary>
    public class Supplier : BaseEntity, ILocalizedEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedOnUtc { get; set; }
    }
}