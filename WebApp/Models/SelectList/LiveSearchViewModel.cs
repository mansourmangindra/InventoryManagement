using Common.DataTransferObjects._Core.ReferenceData;

namespace WebApp.Models.SelectList
{
    public class LiveSearchViewModel
    {
        public string ControlId { get; set; }
        public ReferenceDataDetail SelectedItem { get; set; }
        public bool ReadOnly { get; set; }
    }
}