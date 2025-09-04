namespace Application.DTOs;

public class CreatePostDto
{
    public string? Title { get; set; }
    public string? Content { get; set; }
    public int UserId { get; set; }
    public int CategoryId { get; set; }
}