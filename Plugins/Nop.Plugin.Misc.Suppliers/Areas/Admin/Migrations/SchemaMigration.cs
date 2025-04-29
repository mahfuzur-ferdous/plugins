using FluentMigrator;
using Nop.Data.Extensions;
using Nop.Data.Migrations;
using Nop.Plugin.Misc.Suppliers.Areas.Admin.Domain;
namespace Nop.Plugin.Misc.Suppliers.Areas.Admin.Migrations;

[NopSchemaMigration("2025/04/27 06:16:58:1687541", "Other.SupplierTracker base schema", MigrationProcessType.Installation)]
public class SchemaMigration : AutoReversingMigration
{
    public override void Up()
    {
        if (!Schema.Table(nameof(Domain.Supplier)).Exists())
            Create.TableFor<Domain.Supplier>();
        if (!Schema.Table(nameof(ProductSupplierMapping)).Exists())
            Create.TableFor<ProductSupplierMapping>();
    }
}
