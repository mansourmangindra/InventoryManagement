using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Models.SelectList
{
    public class SelectListViewModel
    {
        public string ControlId { get; set; }
        public bool ReadOnly { get; set; }
        public virtual IEnumerable<SelectListItem> ListItems { get; set; }
    }
}