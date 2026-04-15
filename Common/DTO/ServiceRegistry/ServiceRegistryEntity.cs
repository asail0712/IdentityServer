using AetherCore.Entities;

namespace Common.DTO.ServiceRegistry
{
    public class ServiceRegistryEntity : IDBEntity
    {        
        public string Id                { get; set; }              = string.Empty;
        public DateTime CreatedAt       { get; set; }             // 建立時間
        public DateTime UpdatedAt       { get; set; }             // 更新時間

        public string RegistryId        { get; set; } = string.Empty;
        public string RegistrySecret    { get; set; } = string.Empty;
        public string Endpoint          { get; set; } = string.Empty;
        public int ExpiresInMin         { get; set; }

        public ServiceRegistryEntity() 
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
