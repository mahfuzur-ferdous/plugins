namespace Nop.Plugin.Misc.Suppliers.Areas.Admin.Utilities;
public static class SupplierLocaleResources
{
    public static Dictionary<string, string> GetAll() => new Dictionary<string, string>
    {
        ["Admin.Suppliers"] = "Suppliers",
        ["Admin.Suppliers.Addnew"] = "Add New Supplier",
        ["Admin.Suppliers.Info"] = "Supplier Details",
        ["Admin.Suppliers.Backtolist"] = "Back to Supplier List",
        ["Admin.Supplier.Added"] = "Supplier added successfully!",
        ["Admin.Vendors.Updated"] = "Supplier updated successfully!",
        ["Admin.Suppliers.EditSupplierDetails"] = "Update Supplier Details",
        ["Admin.Suppliers.Fields.Name.Required"] = "Please enter the supplier's name.",
        ["Admin.Suppliers.Fields.Email.Required"] = "Please enter a valid email address.",
        ["Admin.Suppliers.Fields.Phone.Required"] = "Please provide the supplier's phone number.",
        ["Admin.Suppliers.Fields.Address.Required"] = "Please provide the supplier's address.",
        ["Admin.Suppliers.Fields.Description.Required"] = "Please write a short description for the supplier.",
        ["Admin.Common.WrongEmail"] = "This email address doesn't look right.",
        ["Admin.Common.WrongPhone"] = "Please enter a valid Bangladeshi phone number.",
        ["Admin.Common.ExitDescriptionLength"] = "The description should be under 250 characters.",
        ["Plugins.Misc.Suppliers.Fields.Name"] = "Name",
        ["Plugins.Misc.Suppliers.Fields.Email"] = "Email",
        ["Plugins.Misc.Suppliers.Fields.Description"] = "Description",
        ["Plugins.Misc.Suppliers.Fields.Active"] = "Active",
        ["Plugins.Misc.Suppliers.Fields.CreatedOn"] = "Created On",
        ["Plugins.Misc.Suppliers.Added"] = "Great! The supplier was added successfully.",
        ["Plugins.Misc.Suppliers.Updated"] = "The supplier’s information has been updated.",
        ["Plugins.Misc.Suppliers.Deleted"] = "The supplier was deleted successfully.",
        ["Plugins.Misc.Suppliers.List.SearchName"] = "Supplier Name",
        ["Plugins.Misc.Suppliers.List.SearchEmail"] = "Supplier Email",
        ["Plugins.Misc.Suppliers.Menu.Suppliers"] = "Suppliers",
        ["Plugins.Misc.Suppliers.Fields.Email.Hint"] = "Enter a valid email address.",
        ["Plugins.Misc.Suppliers.List.SearchName.Hint"] = "Type the supplier’s name to search.",
        ["Admin.Supplier.Widget"] = "Supplier",
        ["Admin.Supplier.Widget.Description"] = "Suppliers are the people or companies that provide products to your store. For example, if you sell electronics, you might work with different suppliers for phones, laptops, and accessories. You can add suppliers here and assign them to products on the product edit page. Managing suppliers helps you stay organized, track where your items come from, and make reordering easier.",
        ["Admin.Supplier.Widget.Message"] = "Please save the product first before adding a supplier."

    };
}
