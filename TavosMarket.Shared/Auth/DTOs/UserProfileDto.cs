namespace TavosMarket.Shared.Auth.DTOs;

public sealed class UserProfileDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }

    public UserProfileDto() { }

    public UserProfileDto(Guid id, string email, string userName, string? firstName, string? lastName, string? phoneNumber)
    {
        Id = id;
        Email = email;
        UserName = userName;
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
    }
}
