using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Catalog;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Misc.Suppliers.Areas.Admin.Domain;

namespace Nop.Plugin.Misc.Suppliers.Areas.Admin.Infrastructure.Mapper
{
    public class ProductSupplierMappingBuilder : NopEntityBuilder<ProductSupplierMapping>
    {
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(ProductSupplierMapping.ProductId)).AsInt32().NotNullable()
                    .ForeignKey("FK_ProductSupplierMapping_Product", nameof(Product), "Id")
                .WithColumn(nameof(ProductSupplierMapping.SupplierId)).AsInt32().NotNullable()
                    .ForeignKey("FK_ProductSupplierMapping_Supplier", "Supplier", "Id");
        }
    }
}
