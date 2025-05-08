using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Models.SelectList;
using WebApp.Services.Interfaces;

namespace WebApp.ViewComponents.SelectList
{
    public class SelectListViewComponent : ViewComponent
    {
        private readonly IReferenceDataService _referenceDataService;
        public SelectListViewComponent(IReferenceDataService referenceDataService)
        {
            _referenceDataService = referenceDataService;
        }

        public async Task<IViewComponentResult> InvokeAsync(SelectListParam selectListViewComponentParam)
        {
            IEnumerable<SelectListItem> selectListItems = await _referenceDataService
                .GetReferenceDataSelectList(
                selectListViewComponentParam.ApiResource,
                selectListViewComponentParam.ApiEndpoint,
                selectListViewComponentParam.SelectedValue,
                selectListViewComponentParam.EnableCache);

            SelectListViewModel selectListViewModel = new()
            {
                ControlId = selectListViewComponentParam.ControlId,
                ListItems = selectListItems,
                ReadOnly = selectListViewComponentParam.ReadOnly
            };

            return View("~/Views/_Core/SelectList/_SelectList.cshtml", selectListViewModel);
        }
    }
}