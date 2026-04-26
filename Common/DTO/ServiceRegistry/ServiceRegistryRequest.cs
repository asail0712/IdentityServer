namespace Common.DTO.ServiceRegistry
{
    public class ServiceRegistryRequest
    {
        public string RegistryId        { get; set; } = string.Empty;
        public string RegistrySecret    { get; set; } = string.Empty;
        public string Endpoint          { get; set; } = string.Empty;
        public int ExpiresInMin         { get; set; }
    }
}
