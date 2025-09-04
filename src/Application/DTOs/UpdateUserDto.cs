using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

/// <summary>
/// Data transfer object for updating an existing user.
/// </summary>
public class UpdateUserDto
{
    /// <summary>
    /// Gets or sets the name of the user.
    /// </summary>
    [Required(ErrorMessage = "User name is required")]
    [StringLength(100, ErrorMessage = "User name cannot exceed 100 characters")]
    public string? Name { get; set; }
    
    /// <summary>
    /// Gets or sets the email address of the user.
    /// </summary>
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [StringLength(255, ErrorMessage = "Email cannot exceed 255 characters")]
    public string? Email { get; set; }
}