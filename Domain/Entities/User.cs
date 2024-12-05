namespace XMP.Domain.Entities
{
    public class User
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string? ResetPasswordToken { get; set; }
        public DateTime? ResetPasswordSentAt { get; set; }
        public DateTime? RememberCreatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? Avatar { get; set; }
       //public string? Username { get; set; }
        public string? Name { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Gender { get; set; }
        public string? MobileNumber { get; set; }
        public string? Role { get; set; }
        public bool? IsActive { get; set; } = true;
        public string Password { get; set; } = string.Empty;/*
        public List<long> CompanyIds { get; set; } = new List<long>();
        public List<long> ProjectIds { get; set; } = new List<long>();
        public List<long> RoleIds { get; set; } = new List<long>();*/
    }   
}
