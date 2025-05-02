using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator;
using global::Nop.Data.Extensions;
using global::Nop.Data.Migrations;
using Nop.Plugin.Payments.PurchaseOrder.Areas.Admin.Domain;

namespace Nop.Plugin.Payments.PurchaseOrder.Areas.Admin.Migrations
{
    [NopSchemaMigration("2025/05/02 07:16:55:1687541", "Other.PurchaseOrder base schema", MigrationProcessType.Installation)]
    public class SchemaMigration : AutoReversingMigration
    {
        public override void Up()
        {
            if (!Schema.Table(nameof(Domain.PurchaseOrder)).Exists())
                Create.TableFor<Domain.PurchaseOrder>();
            if (!Schema.Table(nameof(PurchaseOrderItem)).Exists())
                Create.TableFor<PurchaseOrderItem>();
        }
    }
}
