using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Models.SelectList;
using WebApp.Services.Interfaces;

namespace WebApp.ViewComponents.SelectList
{
    public class MultiSelectListViewComponent : ViewComponent
    {
        private readonly IReferenceDataService _referenceDataService;
        public MultiSelectListViewComponent(IReferenceDataService referenceDataService)
        {
            _referenceDataService = referenceDataService;
        }

        public async Task<IViewComponentResult> InvokeAsync(MultiSelectListParam multiSelectListParam)
        {
            IEnumerable<SelectListItem> selectListItems = await _referenceDataService.GetReferenceDataMultiSelectList(
                multiSelectListParam.ApiResource,
                multiSelectListParam.ApiEndpoint,
                multiSelectListParam.SelectedValues,
                multiSelectListParam.EnableCache);

            SelectListViewModel selectListViewModel = new()
            {
                ControlId = multiSelectListParam.ControlId,
                ListItems = selectListItems,
                ReadOnly = multiSelectListParam.ReadOnly
            };

            return View("~/Views/_Core/SelectList/_MultiSelectList.cshtml", selectListViewModel);
        }
    }
}