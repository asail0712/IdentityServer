using AetherCore.Entities;

namespace Common.DTO.User
{
    public class UserEntity : IDBEntity
    {        
        public string Id { get; set; }              = string.Empty;
        public DateTime CreatedAt { get; set; }             // 建立時間
        public DateTime UpdatedAt { get; set; }             // 更新時間
        public string DisplayName { get; set; }     = default!;
        public string? AvatarUrl { get; set; }
        public bool IsActive { get; set; }          = true;

        public UserEntity() 
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
