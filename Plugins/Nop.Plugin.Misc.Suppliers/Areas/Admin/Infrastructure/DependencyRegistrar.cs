using Autofac;
using Nop.Plugin.Misc.Suppliers.Areas.Admin.Factories;
using Nop.Plugin.Misc.Suppliers.Areas.Admin.Services;

namespace Nop.Plugin.Misc.Suppliers.Areas.Admin.Infrastructure
{
    public class DependencyRegistrar
    {
        public static void Register(ContainerBuilder builder)
        {
            builder.RegisterType<SupplierService>().As<ISupplierService>().InstancePerLifetimeScope();
            builder.RegisterType<SupplierModelFactory>().As<ISupplierModelFactory>().InstancePerLifetimeScope();
        }
        public int Order => 1;
    }
}
