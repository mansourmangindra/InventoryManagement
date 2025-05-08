using Common.Constants;

namespace WebApp.Models.SelectList
{
    public class MultiSelectListParam
    {
        public string ApiEndpoint { get; set; }
        public ApiResource ApiResource { get; set; } = ApiResource.InventoryApiNamedClient;
        public string ControlId { get; set; } = "ddlSelectList";
        public IEnumerable<string> SelectedValues { get; set; }
        public bool ReadOnly { get; set; } = false;
        public bool EnableCache { get; set; } = true;
    }
}