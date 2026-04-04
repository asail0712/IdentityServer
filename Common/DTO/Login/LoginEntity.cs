using AetherCore.Entities;

namespace Common.DTO.Login
{
    public class LoginEntity : IDBEntity
    {        
        public string Id { get; set; }              = string.Empty;
        public DateTime CreatedAt { get; set; }             // 建立時間
        public DateTime UpdatedAt { get; set; }             // 更新時間
        public bool IsEnabled { get; set; }         = true;
        public string UserId { get; set; }          = default!;
        public string Provider { get; set; }        = default!; // COMMON / LINE / GOOGLE / APPLE

        // 第三方平台登入用
        public string ProviderUserId { get; set; }  = default!;

        // COMMON 登入用
        public string? Account { get; set; }        = string.Empty;
        public string? PasswordHash { get; set; }   = string.Empty;

        public LoginEntity() 
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
