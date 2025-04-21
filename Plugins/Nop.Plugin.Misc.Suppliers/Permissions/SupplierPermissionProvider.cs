//// Nop.Plugin.Misc.Suppliers/Infrastructure/SupplierPermissionProvider.cs
//using System.Collections.Generic;
//using Nop.Core.Domain.Security;
//using Nop.Services.Security;

//namespace Nop.Plugin.Misc.Suppliers.Infrastructure
//{
//    public class SupplierPermissionProvider : IPermissionProvider
//    {
//        public static readonly PermissionRecord ManageSuppliers = new()
//        {
//            Name = "Admin area. Manage Suppliers",
//            SystemName = "ManageSuppliers",
//            Category = "Suppliers"
//        };

//        public IEnumerable<PermissionRecord> GetPermissions()
//        {
//            return new[] { ManageSuppliers };
//        }

//        public HashSet<(string systemRoleName, PermissionRecord permission)> GetDefaultPermissions()
//        {
//            return new HashSet<(string, PermissionRecord)>
//            {
//                ("Administrators", ManageSuppliers)
//            };
//        }
//    }
//}