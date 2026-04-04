using Common.DTO.Login;
using AetherCore.DataAccess;

namespace DataAccess.Interface
{
    public interface ILoginDataAccess : IDataAccess<LoginEntity>
    {
        Task<LoginEntity?> GetByProviderAsync(string provider, string providerUserId);
    }
}
