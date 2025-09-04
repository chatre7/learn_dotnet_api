namespace Application.DTOs;

public class UpdatePostDto
{
    public string? Title { get; set; }
    public string? Content { get; set; }
    public int CategoryId { get; set; }
}