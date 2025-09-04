namespace Application.DTOs;

/// <summary>
/// Data transfer object for updating an existing user.
/// </summary>
public class UpdateUserDto
{
    /// <summary>
    /// Gets or sets the name of the user.
    /// </summary>
    public string? Name { get; set; }
    
    /// <summary>
    /// Gets or sets the email address of the user.
    /// </summary>
    public string? Email { get; set; }
}