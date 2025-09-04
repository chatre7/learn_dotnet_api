using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApi.Controllers;

/// <summary>
/// Controller for managing users.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UsersController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UsersController"/> class.
    /// </summary>
    /// <param name="userService">The user service.</param>
    /// <param name="logger">The logger instance.</param>
    public UsersController(IUserService userService, ILogger<UsersController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    /// <summary>
    /// Gets all users.
    /// </summary>
    /// <returns>A collection of user DTOs.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
        _logger.LogInformation("Getting all users");
        var users = await _userService.GetAllUsersAsync();
        _logger.LogInformation("Retrieved {Count} users", users.Count());
        return Ok(users);
    }

    /// <summary>
    /// Gets a user by its ID.
    /// </summary>
    /// <param name="id">The user ID.</param>
    /// <returns>The user DTO if found; otherwise, NotFound.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUser(int id)
    {
        _logger.LogInformation("Getting user by ID: {Id}", id);
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
        {
            _logger.LogWarning("User not found with ID: {Id}", id);
            return NotFound();
        }
        _logger.LogInformation("User found with ID: {Id}", id);
        return Ok(user);
    }

    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="createUserDto">The DTO containing user creation data.</param>
    /// <returns>The created user DTO.</returns>
    [HttpPost]
    public async Task<ActionResult<UserDto>> CreateUser(CreateUserDto createUserDto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state for user creation");
            return BadRequest(ModelState);
        }
        
        _logger.LogInformation("Creating new user with name: {Name}", createUserDto.Name);
        var user = await _userService.CreateUserAsync(createUserDto);
        _logger.LogInformation("User created successfully with ID: {Id}", user.Id);
        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }

    /// <summary>
    /// Updates an existing user.
    /// </summary>
    /// <param name="id">The ID of the user to update.</param>
    /// <param name="updateUserDto">The DTO containing updated user data.</param>
    /// <returns>The updated user DTO.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, UpdateUserDto updateUserDto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state for user update with ID: {Id}", id);
            return BadRequest(ModelState);
        }
        
        try
        {
            _logger.LogInformation("Updating user with ID: {Id}", id);
            var user = await _userService.UpdateUserAsync(id, updateUserDto);
            _logger.LogInformation("User updated successfully with ID: {Id}", user.Id);
            return Ok(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user with ID: {Id}", id);
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Deletes a user by its ID.
    /// </summary>
    /// <param name="id">The ID of the user to delete.</param>
    /// <returns>NoContent if successful; otherwise, NotFound.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        _logger.LogInformation("Deleting user with ID: {Id}", id);
        var result = await _userService.DeleteUserAsync(id);
        if (!result)
        {
            _logger.LogWarning("User not found for deletion with ID: {Id}", id);
            return NotFound();
        }
        _logger.LogInformation("User deleted successfully with ID: {Id}", id);
        return NoContent();
    }
}