using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Models.SelectList;

namespace WebApp.ViewComponents
{
    public class ActiveMultiSelectListViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(
            string controlId = "ddlSelectList",
            string optionLabel = "--Select--",
            IEnumerable<string> selectedValues = null,
            bool readOnly = false)
        {

            List<SelectListItem> selectListItems = new()
            {
                new SelectListItem() { Value = "true", Text = "Yes", Selected = selectedValues != null && selectedValues.Contains("True") },
                new SelectListItem() { Value = "false", Text = "No", Selected = selectedValues != null && selectedValues.Contains("False") }
            };

            SelectListViewModel selectListViewModel = new()
            {
                ControlId = controlId,
                ListItems = selectListItems,
                ReadOnly = readOnly,
                OptionLabel = optionLabel
            };

            return View("~/Views/_Core/SelectList/_MultiSelectList.cshtml", selectListViewModel);
        }
    }
}
