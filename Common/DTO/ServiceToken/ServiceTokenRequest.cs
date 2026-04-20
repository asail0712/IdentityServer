namespace Common.DTO.ServiceToken
{
    public class ServiceTokenRequest
    {
        public string ServiceId         { get; set; } = string.Empty;
        public string RegistryId        { get; set; } = string.Empty;
        public string RegistrySecret    { get; set; } = string.Empty;
    }
}
