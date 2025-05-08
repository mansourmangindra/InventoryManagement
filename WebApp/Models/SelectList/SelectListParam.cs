using Common.Constants;

namespace WebApp.Models.SelectList
{
    public class SelectListParam
    {
        public string ApiEndpoint { get; set; }
        public ApiResource ApiResource { get; set; } = ApiResource.InventoryApiNamedClient;
        public string ControlId { get; set; } = "ddlSelectList";
        public string SelectedValue { get; set; }
        public bool ReadOnly { get; set; } = false;
        public bool EnableCache { get; set; } = true;
    }
}