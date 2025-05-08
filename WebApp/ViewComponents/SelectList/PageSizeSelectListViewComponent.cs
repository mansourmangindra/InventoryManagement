using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Models.SelectList;

namespace WebApp.ViewComponents.SelectList
{
    public class PageSizeSelectListViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(
            string controlId = "ddlSelectList",
            string selectedValues = "10",
            string pageSize = "10",
            bool readOnly = false)
        {
            List<SelectListItem> selectListItems = new List<SelectListItem>();


            if (pageSize == "10")
            {
                selectListItems = new()
                {
                    new SelectListItem() { Value = "10", Text = "10", Selected = selectedValues != null && selectedValues == "10" },
                    new SelectListItem() { Value = "20", Text = "20", Selected = selectedValues != null && selectedValues == "20" },
                    new SelectListItem() { Value = "50", Text = "50", Selected = selectedValues != null && selectedValues == "50" },
                    new SelectListItem() { Value = "100", Text = "100", Selected = selectedValues != null && selectedValues == "100" },
                    new SelectListItem() { Value = "500", Text = "500", Selected = selectedValues != null && selectedValues == "500" }
                };
            }
            else if (pageSize == "12")
            {
                selectListItems = new()
                {
                    new SelectListItem() { Value = "12", Text = "12", Selected = selectedValues != null && selectedValues == "12" },
                    new SelectListItem() { Value = "24", Text = "24", Selected = selectedValues != null && selectedValues == "24" },
                    new SelectListItem() { Value = "58", Text = "58", Selected = selectedValues != null && selectedValues == "58" },
                    new SelectListItem() { Value = "100", Text = "100", Selected = selectedValues != null && selectedValues == "100" },
                    new SelectListItem() { Value = "500", Text = "500", Selected = selectedValues != null && selectedValues == "500" }
                };
            }

            SelectListViewModel selectListViewModel = new()
            {
                ControlId = controlId,
                ListItems = selectListItems,
                ReadOnly = readOnly
            };

            return View("~/Views/_Core/SelectList/_SelectList.cshtml", selectListViewModel);
        }
    }
}