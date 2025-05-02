using FluentMigrator.Builders.Create.Table;
using Nop.Core;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Payments.PurchaseOrder.Areas.Admin.Domain;

namespace Nop.Plugin.Payments.PurchaseOrder.Areas.Admin.Infrastructure.Mapping
{
    /// <summary>
    /// Mapping class for PurchaseOrder entity 
    /// </summary>
    public class PurchaseOrderMapBuilder : NopEntityBuilder<Domain.PurchaseOrder>
    {
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(Domain.PurchaseOrder.PurchaseOrderNumber)).AsString(50).NotNullable();
        }
    }
}

