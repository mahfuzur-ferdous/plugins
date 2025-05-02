using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Catalog;
using Nop.Data.Extensions;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Payments.PurchaseOrder.Areas.Admin.Domain;

namespace Nop.Plugin.Payments.PurchaseOrder.Areas.Admin.Infrastructure.Mapping
{
     /// <summary>
     /// Mapping class for PurchaseOrderItem entity (used by FluentMigrator)
     /// </summary>
     public class PurchaseOrderItemMap : NopEntityBuilder<PurchaseOrderItem>
     {
         public override void MapEntity(CreateTableExpressionBuilder table)
         {
             table
                .WithColumn(nameof(PurchaseOrderItem.PurchaseOrderId)).AsInt32().NotNullable().ForeignKey<Domain.PurchaseOrder>()
                .WithColumn(nameof(PurchaseOrderItem.ProductId)).AsInt32().NotNullable()
                .WithColumn(nameof(PurchaseOrderItem.Quantity)).AsInt32().NotNullable();
        }
     }
}
