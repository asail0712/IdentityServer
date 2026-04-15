using AetherCore.Entities;

namespace Common.DTO.ServiceToken
{
    public class ServiceTokenEntity : IDBEntity
    {        
        public string Id                { get; set; } = string.Empty;
        public DateTime CreatedAt       { get; set; }             // 建立時間
        public DateTime UpdatedAt       { get; set; }             // 更新時間

        public string ServiceId         { get; set; } = string.Empty;
        public string RegistryId        { get; set; } = string.Empty;
        public bool Success             { get; set; }

        public ServiceTokenEntity() 
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
