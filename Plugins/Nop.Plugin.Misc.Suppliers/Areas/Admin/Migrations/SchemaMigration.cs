using FluentMigrator;
using Nop.Data.Extensions;
using Nop.Data.Migrations;
namespace Nop.Plugin.Misc.Suppliers.Areas.Admin.Migrations;

[NopSchemaMigration("2020/05/27 08:40:55:1687541", "Other.SupplierTracker base schema", MigrationProcessType.Installation)]
public class SchemaMigration : AutoReversingMigration
{
    public override void Up()
    {
        Create.TableFor<Domain.Supplier>();
    }
}
