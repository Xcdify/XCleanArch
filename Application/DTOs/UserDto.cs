namespace XMP.Application.DTOs
{
   public class UserResponseDto
{
    public long Id { get; set; }
    public string? Email { get; set; }
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
    public bool? IsActive { get; set; }
    /*public List<long> CompanyIds { get; set; } = new List<long>();

    public List<long> ProjectIds { get; set; } = new List<long>();

    public List<long> RoleIds { get; set; } = new List<long>();*/
}
    public class CreateUserDto
{
    // Required fields for user creation
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public required string Username { get; set; }
    
    // Optional fields
    public string? Name { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Gender { get; set; }
    public string? MobileNumber { get; set; }
    public string? Role { get; set; }
    public bool IsActive { get; set; } = true;
/*    public List<long> CompanyIds { get; set; } = new List<long>();
    public List<long> ProjectIds { get; set; } = new List<long>();
    public List<long> RoleIds { get; set; } = new List<long>();*/
}
public class UpdateUserDto
{
    public required long Id { get; set; }
    
    // All fields optional for updates
    public string? Email { get; set; }
    public string? PasswordHash { get; set; }
    public string? Username { get; set; }
    public string? Name { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Gender { get; set; }
    public string? MobileNumber { get; set; }
    public string? Role { get; set; }
    public bool? IsActive { get; set; }
/*    public List<long>? CompanyIds { get; set; }
    public List<long>? ProjectIds { get; set; }
    public List<long>? RoleIds { get; set; }*/
}
}
