using AetherCore.Repository;
using AetherCore.Utility.Attributes;
using AetherCore.Utility.Caches;
using Common.DTO.Login;
using DataAccess.Interface;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Repository.Interface;

namespace Repository
{
    [AutoInject(ServiceLifetime.Scoped)]
    public class LoginRepository : GenericRepository<LoginEntity, ILoginDataAccess>, ILoginRepository
    {
        public LoginRepository(ILoginDataAccess dataAccess, IMemoryCache memoryCache, IOptions<CacheSettings> cacheSettings) 
            : base(dataAccess, memoryCache, cacheSettings)
        {

        }

        public async Task<LoginEntity> GetByProviderAsync(string provider, string providerUserId)
        {
            string cacheKey = $"LoginEntity_{provider}_{providerUserId}";
            if (_cache.TryGetValue(cacheKey, out LoginEntity cachedEntity))
            {
                return cachedEntity;
            }

            var entity = await _dataAccess.GetByProviderAsync(provider, providerUserId);
            if (entity != null)
            {
                _cache.Set(cacheKey, entity, TimeSpan.FromMinutes(_cacheDurationMinutes));
            }
            return entity;
        }
    }
}
