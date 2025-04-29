//using Microsoft.AspNetCore.Mvc.Razor;
//using System.Collections.Generic;
//using System.Linq;

//namespace Nop.Plugin.Misc.Suppliers.Areas.Admin.Infrastructure
//{
//    public class ViewLocationExpander : IViewLocationExpander
//    {
//        public void PopulateValues(ViewLocationExpanderContext context)
//        {
//        }
//        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
//        {
//            if (context.AreaName == "Admin")
//                viewLocations = new string[]
//                {
//                    $"/Plugins/Misc.Suppliers/Areas/Admin/Views/{{0}}.cshtml",
//                    $"/Plugins/Misc.Suppliers/Areas/Admin/Views/SupplierAdmin/{{0}}.cshtml",
//                    $"/Plugins/Misc.Suppliers/Areas/Admin/Views/Shared/{{0}}.cshtml"
//                }.Concat(viewLocations);
//            return viewLocations;
//        }
//    }
//}