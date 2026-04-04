using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;
using AetherCore.Entities;

namespace Common.DTO.Login
{
    public class LoginDocument : IEntity, IDBEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }              = string.Empty;
        public DateTime CreatedAt { get; set; }                                     // 建立時間
        public DateTime UpdatedAt { get; set; }                                     // 更新時間

        public bool IsEnabled { get; set; }         = true;
        public string UserId { get; set; }          = default!;
        public string Provider { get; set; }        = default!; // COMMON / LINE / GOOGLE / APPLE

        // 第三方平台登入用
        public string ProviderUserId { get; set; }  = default!;

        // COMMON 登入用
        public string? Account { get; set; }        = string.Empty;
        public string? PasswordHash { get; set; }   = string.Empty;

        // 實作 IEntity
        public object GenerateNewID()   => ObjectId.GenerateNewId().ToString()!;
        public bool HasDefaultID()      => string.IsNullOrEmpty(Id);
    }
}
