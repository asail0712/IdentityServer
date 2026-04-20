using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;
using AetherCore.Entities;

namespace Common.DTO.ServiceToken
{
    public class ServiceTokenDocument : IEntity, IDBEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id                { get; set; } = string.Empty;
        public DateTime CreatedAt       { get; set; }                                     // 建立時間
        public DateTime UpdatedAt       { get; set; }                                     // 更新時間

        public string ServiceId         { get; set; } = string.Empty;
        public string RegistryId        { get; set; } = string.Empty;
        public bool Success             { get; set; }

        // 實作 IEntity
        public object GenerateNewID()   => ObjectId.GenerateNewId().ToString()!;
        public bool HasDefaultID()      => string.IsNullOrEmpty(Id);
    }
}
