namespace Application.DTOs;

public class CommentDto
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public int UserId { get; set; }
    public int PostId { get; set; }
    public DateTime CreatedAt { get; set; }
}