using Common.DTO.Login;

using AetherCore.Repository;

namespace Repository.Interface
{
    public interface ILoginRepository : IRepository<LoginEntity>
    {
        public Task<LoginEntity> GetByProviderAsync(string provider, string providerUserId);
    }
}
