﻿using Microsoft.AspNetCore.Mvc.Razor;
using Nop.Core.Infrastructure;
using Nop.Web.Framework.Themes;

namespace Nop.Plugin.Misc.Suppliers.Areas.Admin.Infrastructure;
public class PluginViewLocationExpander : IViewLocationExpander
{
    protected const string THEME_KEY = "nop.themename";
    public void PopulateValues(ViewLocationExpanderContext context)
    {
        context.Values[THEME_KEY] = EngineContext.Current.Resolve<IThemeContext>().GetWorkingThemeNameAsync().Result;
    }

    public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
    {
        if (context.AreaName == "Admin")
        {
            viewLocations = new string[]
            {
                $"/Plugins/Misc.Suppliers/Areas/Admin/Views/{{0}}.cshtml",
                $"/Plugins/Misc.Suppliers/Areas/Admin/Views/{{1}}/{{0}}.cshtml"
            }.Concat(viewLocations);
        }
        else
        {
            viewLocations = new string[]
            {
                $"/Plugins/Nop.Plugin.Misc.Suppliers/Areas/Admin/Views/{{0}}.cshtml",
                $"/Plugins/Nop.Plugin.Misc.Suppliers/Areas/Admin/Views/{{1}}/{{0}}.cshtml"
            }.Concat(viewLocations);
        }

        return viewLocations;
    }
}