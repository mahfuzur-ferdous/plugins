﻿using FluentMigrator.Builders.Create.Table;
using Nop.Data.Mapping.Builders;

namespace Nop.Plugin.Misc.Suppliers.Areas.Admin.Infrastructure.Mapper
{
    /// <summary>
    /// Mapping class for Supplier entity
    /// </summary>
    public class SupplierMap : NopEntityBuilder<Domain.Supplier>
    {
        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(Domain.Supplier.Name)).AsString(400).NotNullable()
                .WithColumn(nameof(Domain.Supplier.Email)).AsString(400).NotNullable()
                .WithColumn(nameof(Domain.Supplier.Description)).AsString(int.MaxValue).Nullable()
                .WithColumn(nameof(Domain.Supplier.Active)).AsBoolean().NotNullable()
                .WithColumn(nameof(Domain.Supplier.CreatedOnUtc)).AsDateTime2().NotNullable();
        }
    }
}