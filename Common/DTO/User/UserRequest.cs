namespace Common.DTO.User
{
    public class UserRequest
    {
        public string DisplayName { get; set; } = default!;
        public string? AvatarUrl { get; set; }
        public bool IsActive { get; set; }      = true;
    }
}
