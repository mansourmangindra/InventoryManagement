using Common.Constants;
using Common.DataTransferObjects.UserRole;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Security.Principal;
using WebApp.Services.Interfaces;
using Common.DataTransferObjects._Core.AppSettings;
using Common.DataTransferObjects._Core.ReferenceData;

namespace WebApp.Services
{
    public class SecurityService : ISecurityService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly CacheSetting _cacheSetting;
        private readonly HttpClient _httpClient;
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SecurityService(IMemoryCache memoryCache, CacheSetting cacheSetting, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment environment)
        {
            _memoryCache = memoryCache;
            _cacheSetting = cacheSetting;
            _httpClient = httpClientFactory.CreateClient(HttpClientConstant.InventoryApiNamedClient);
            _httpContextAccessor = httpContextAccessor;
            _environment = environment;
        }

        public async Task<IEnumerable<UserRoleWithModule>> GetRolesWithModule(IPrincipal principal)
        {
            IEnumerable<UserRoleWithModule> userRoleWithModuleDetails = await GetUserRoleWithModuleFromCache(principal);
            return userRoleWithModuleDetails;
        }

        public async Task<IEnumerable<ReferenceDataDetail>> GetRoles(IPrincipal principal)
        {
            IEnumerable<UserRoleWithModule> userRoleWithModuleDetails = await GetUserRoleWithModuleFromCache(principal);
            IEnumerable<ReferenceDataDetail> roleDetails = userRoleWithModuleDetails.Select(x => new ReferenceDataDetail()
            {
                Value = x.RoleDetail.Value,
                Name = x.RoleDetail.Name
            });

            return roleDetails;
        }

        public async Task<IEnumerable<ReferenceDataDetail>> GetModules(IPrincipal principal)
        {
            IEnumerable<UserRoleWithModule> userRoleWithModuleDetails = await GetUserRoleWithModuleFromCache(principal);
            IEnumerable<ReferenceDataDetail> moduleDetails = userRoleWithModuleDetails
                .SelectMany(r => r.ModuleDetails)
                .Select(m => new ReferenceDataDetail()
                {
                    Value = m.Value,
                    Name = m.Name
                })
                .Distinct();

            return moduleDetails;
        }

        public async Task<UserRoleWithModule> GetSelectedRole(IPrincipal principal)
        {
            IEnumerable<UserRoleWithModule> userRoleWithModuleDetails = await GetUserRoleWithModuleFromCache(principal);
            if (userRoleWithModuleDetails == null || !userRoleWithModuleDetails.Any())
            {
                return null;
            }

            if (!userRoleWithModuleDetails.Any(ur => ur.Selected))
            {
                await SetSelectedRole(principal, 0);
            }

            UserRoleWithModule selectedUserRoleWithModule = userRoleWithModuleDetails.SingleOrDefault(ur => ur.Selected);
            return selectedUserRoleWithModule;
        }

        public async Task<bool> WithRole(short[] roleIds, IPrincipal principal)
        {
            IEnumerable<ReferenceDataDetail> roleDetails = await GetRoles(principal);
            return roleDetails.Any(r => roleIds.Contains(short.Parse(r.Value.ToString())));
        }


        public async Task<bool> WithModule(short[] moduleIds, IPrincipal principal)
        {
            IEnumerable<ReferenceDataDetail> moduleDetails = await GetModules(principal);
            return moduleDetails.Any(r => moduleIds.Contains(short.Parse(r.Value.ToString())));
        }

        private async Task<IEnumerable<UserRoleWithModule>> GetUserRoleWithModuleFromCache(IPrincipal principal)
        {
            string cacheName = $"UserRoleWithModuleDetails_{principal.Identity.Name}";
            if (!_memoryCache.TryGetValue(cacheName, out IEnumerable<UserRoleWithModule> userRoleWithModuleDetails))
            {
                var response = await _httpClient.GetAsync($"api/UserRole/UserPrincipalName/{principal.Identity.Name}/UserRoleWithModuleDetails");

                if (response.IsSuccessStatusCode)
                {
                    userRoleWithModuleDetails = JsonConvert.DeserializeObject<IEnumerable<UserRoleWithModule>>(await response.Content.ReadAsStringAsync());

                    MemoryCacheEntryOptions cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(_cacheSetting.ExpirationMinutes));
                    _memoryCache.Set(cacheName, userRoleWithModuleDetails, cacheEntryOptions);
                }
                else
                {
                    return Enumerable.Empty<UserRoleWithModule>();
                }
            }
            return userRoleWithModuleDetails;
        }

        //public async Task<bool> ActiveUser(IPrincipal principal)
        //{
        //    UserSearchFilter filter = new UserSearchFilter();
        //    filter.SearchKeyword = principal.Identity.Name;
        //    var response = await _httpClient.GetAsync($"api/User/UserListSearch?{filter.GetQueryString()}");
        //    if (response.IsSuccessStatusCode)
        //    {
        //        PagedList<UserDetail> userList = JsonConvert.DeserializeObject<PagedList<UserDetail>>(await response.Content.ReadAsStringAsync());

        //        if (userList.Count > 0)
        //        {
        //            if (userList[0].Active)
        //            {
        //                return true;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        ErrorMessage errorMessage = await response.GetErrorMessage();
        //        throw new ArgumentException(errorMessage.Message);
        //    }

        //    return false;
        //}

        public async Task SetSelectedRole(IPrincipal principal, int roleId)
        {
            IEnumerable<UserRoleWithModule> userRoleWithModuleDetails = await GetUserRoleWithModuleFromCache(principal);

            if (userRoleWithModuleDetails != null && userRoleWithModuleDetails.Any())
            {
                userRoleWithModuleDetails.Where(ur => ur.Selected).ToList().ForEach(ur => ur.Selected = false);
                if (roleId == 0)
                {
                    UserRoleWithModule userRoleWithModule = userRoleWithModuleDetails.OrderBy(ur => ur.RoleDetail.Name).FirstOrDefault();
                    userRoleWithModule.Selected = true;
                }
                else
                {
                    UserRoleWithModule userRoleWithModule = userRoleWithModuleDetails.SingleOrDefault(ur => long.Parse(ur.RoleDetail.Value.ToString()) == roleId);
                    if (userRoleWithModule != null)
                    {
                        userRoleWithModule.Selected = true;
                    }
                }

                UpdateUserRoleWithModuleCache(principal, userRoleWithModuleDetails);
            }
        }

        static private void UpdateUserRoleWithModuleCache(IPrincipal principal, IEnumerable<UserRoleWithModule> userRoleWithModuleDetails)
        {
            string cacheName = $"UserRoleWithModuleDetails_{principal.Identity.Name}";
        }

        
        public class DateTimeConverter : JsonConverter<DateTime>
        {
            public override DateTime ReadJson(JsonReader reader, Type objectType, DateTime existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.Null)
                {
                    return DateTime.MinValue;
                }

                return Convert.ToDateTime(reader.Value);
            }

            public override void WriteJson(JsonWriter writer, DateTime value, JsonSerializer serializer)
            {}
        }

    }
}
