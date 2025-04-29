using Nop.Core;
using Nop.Core.Domain.Catalog;


namespace Nop.Plugin.Misc.Suppliers.Areas.Admin.Domain
{
    public class ProductSupplierMapping : BaseEntity
    {
        public int ProductId { get; set; }
        public int SupplierId { get; set; }



    }
}
