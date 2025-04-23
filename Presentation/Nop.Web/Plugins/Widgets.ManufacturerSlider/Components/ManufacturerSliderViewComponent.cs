using Microsoft.AspNetCore.Mvc;
using Nop.Services.Catalog;
using Nop.Services.Media;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Widgets.ManufacturerSlider.Components
{
    public class ManufacturerSliderViewComponent : NopViewComponent
    {
        private readonly IManufacturerService _manufacturerService;
        private readonly IPictureService _pictureService;

        public ManufacturerSliderViewComponent(
            IManufacturerService manufacturerService,
            IPictureService pictureService)
        {
            _manufacturerService = manufacturerService;
            _pictureService = pictureService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
        {
            var manufacturers = await _manufacturerService.GetAllManufacturersAsync(showHidden: false);

            var pictureUrls = new List<string>();
            foreach (var manufacturer in manufacturers)
            {
                var pictureUrl = await _pictureService.GetPictureUrlAsync(manufacturer.PictureId);
                pictureUrls.Add(pictureUrl);
            }
            return View("~/Plugins/Widgets.ManufacturerSlider/Views/ViewManufacturers.cshtml", pictureUrls);
        }
    }
}
