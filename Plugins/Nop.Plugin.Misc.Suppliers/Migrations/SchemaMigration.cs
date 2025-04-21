using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator;
using Microsoft.AspNetCore.Http.HttpResults;
using Nop.Data.Extensions;
using Nop.Data.Migrations;
using Nop.Plugin.Misc.Suppliers.Domain;
namespace Nop.Plugin.Misc.Suppliers.Migrations;


[NopSchemaMigration("2020/05/27 08:40:55:1687541", "Other.SupplierTracker base schema", MigrationProcessType.Installation)]
public class SchemaMigration : AutoReversingMigration
{
    public override void Up()
    {
        Create.TableFor<Domain.Supplier>();
    }
}
