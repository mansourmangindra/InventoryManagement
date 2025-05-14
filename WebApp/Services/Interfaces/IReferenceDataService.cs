using Common.Constants;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Services.Interfaces
{
    public interface IReferenceDataService
    {
        Task<IEnumerable<SelectListItem>> GetReferenceDataMultiSelectList(ApiResource apiResource, string apiEndpoint, IEnumerable<string> selectedValues, bool enableCache);
        Task<IEnumerable<SelectListItem>> GetReferenceDataSelectList(ApiResource apiResource, string apiEndpoint, string selectedValue, bool enableCache);
        string ConvertBoleanToYesOrNo(bool bolean);
    }
}
