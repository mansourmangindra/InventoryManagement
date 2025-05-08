using Common.Constants;
using Common.DataTransferObjects._Core.AppSettings;
using Common.DataTransferObjects._Core.ErrorLog;
using Common.DataTransferObjects._Core.ReferenceData;
using Common.Extension;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using WebApp.Services.Interfaces;

namespace WebApp.Services
{
    public class ReferenceDataService : IReferenceDataService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly CacheSetting _cacheSetting;
        private readonly HttpClient _httpClientInventory;

        public ReferenceDataService(IHttpClientFactory httpClientFactory, IMemoryCache memoryCache, CacheSetting cacheSetting)
        {
            _memoryCache = memoryCache;
            _cacheSetting = cacheSetting;
            _httpClientInventory = httpClientFactory.CreateClient(HttpClientConstant.InventoryApiNamedClient);
        }

        public async Task<IEnumerable<SelectListItem>> GetReferenceDataSelectList(ApiResource apiResource, string apiEndpoint, string selectedValue, bool enableCache)
        {
            IEnumerable<ReferenceDataDetail> referenceDataDetails = await GetReferenceDataFromApi(apiResource, apiEndpoint, enableCache);

            IEnumerable<SelectListItem> selectListItems = referenceDataDetails.Select(r => new SelectListItem()
            {
                Value = r.Value.ToString(),
                Text = r.Name,
                Selected = selectedValue != null && selectedValue == r.Value.ToString()
            });

            return selectListItems;
        }

        public async Task<IEnumerable<SelectListItem>> GetReferenceDataMultiSelectList(ApiResource apiResource, string apiEndpoint, IEnumerable<string> selectedValues, bool enableCache)
        {
            IEnumerable<ReferenceDataDetail> referenceDataDetails = await GetReferenceDataFromApi(apiResource, apiEndpoint, enableCache);

            IEnumerable<SelectListItem> selectListItems = referenceDataDetails.Select(r => new SelectListItem()
            {
                Value = r.Value.ToString(),
                Text = r.Name,
                Selected = selectedValues != null && selectedValues.Contains(r.Value.ToString())
            });

            return selectListItems;
        }

        private async Task<IEnumerable<ReferenceDataDetail>> GetReferenceDataFromApi(ApiResource apiResource, string apiEndpoint, bool enableCache)
        {
            string cacheKey = $"{apiEndpoint}_{apiResource}";

            if (!_memoryCache.TryGetValue(cacheKey, out IEnumerable<ReferenceDataDetail> referenceDataDetails))
            {
                referenceDataDetails = await GetApiResponseAsync(apiResource, apiEndpoint);

                if (enableCache && (referenceDataDetails != null || referenceDataDetails.Any()))
                {
                    MemoryCacheEntryOptions cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(_cacheSetting.ExpirationMinutes));
                    _memoryCache.Set(cacheKey, referenceDataDetails, cacheEntryOptions);
                }
            }

            return referenceDataDetails;
        }

        private async Task<IEnumerable<ReferenceDataDetail>> GetApiResponseAsync(ApiResource apiResource, string endpoint)
        {
            HttpClient httpClient;
            switch (apiResource)
            {
                case ApiResource.InventoryApiNamedClient:
                    httpClient = _httpClientInventory;
                    break;
                default:
                    throw new ArgumentException($"{nameof(apiResource)} is invalid");
            }

            HttpResponseMessage response = await httpClient.GetAsync(endpoint);

            if (response.IsSuccessStatusCode)
            {
                IEnumerable<ReferenceDataDetail> result = JsonConvert.DeserializeObject<IEnumerable<ReferenceDataDetail>>(await response.Content.ReadAsStringAsync());
                return result;
            }

            ErrorMessage errorMessage = await response.GetErrorMessage();
            throw new ArgumentException(errorMessage.Message);
        }
    }
}
