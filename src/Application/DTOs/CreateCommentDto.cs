namespace Application.DTOs;

public class CreateCommentDto
{
    public string Content { get; set; } = string.Empty;
    public int UserId { get; set; }
    public int PostId { get; set; }
}