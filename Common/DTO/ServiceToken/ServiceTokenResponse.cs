namespace Common.DTO.ServiceToken
{
    public class ServiceTokenResponse
    {
        public string Id            { get; set; } = string.Empty;
        public string ServiceId     { get; set; } = string.Empty;
        public string RegistryId    { get; set; } = string.Empty;
        public string ServiceToken  { get; set; } = string.Empty;
        public bool Success         { get; set; }
    }
}
