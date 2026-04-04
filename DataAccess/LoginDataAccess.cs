using AetherCore.DataAccess;
using AetherCore.Utility.Attributes;
using AutoMapper;
using Common;
using Common.DTO.Login;
using DataAccess.Interface;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using MongoDB.Entities;

namespace DataAccess
{
    [AutoInject(ServiceLifetime.Scoped)]
    public class LoginDataAccess : MongoEntityDataAccess<LoginEntity, LoginDocument>, ILoginDataAccess
    {
        public LoginDataAccess(IMapper mapper)
            : base(mapper)
        {
        }

        public async Task<LoginEntity?> GetByProviderAsync(string provider, string providerUserId)
        {
            var builder = Builders<LoginDocument>.Filter;
            var filter  = builder.Eq(x => x.Provider, provider) &
                            builder.Eq(x => x.IsEnabled, true);

            filter      &= provider switch
            {
                ProviderDefine.Password => builder.Eq(x => x.Account, providerUserId),
                _ => builder.Eq(x => x.ProviderUserId, providerUserId)
            };

            var document = await DB.Find<LoginDocument>()
                .Match(filter)
                .ExecuteFirstAsync();

            return document == null ? null : _mapper.Map<LoginEntity>(document);
        }
    }
}
