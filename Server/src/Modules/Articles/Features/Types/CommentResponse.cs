using BlogBackend.Modules.Articles.Domain;

namespace BlogBackend.Modules.Articles.Features.Types;

public record CommentResponse
{
    public string Id { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
    public string Body { get; init; }
    public Author Author { get; init; }
    public CommentResponse(Comment comment, Author author)
    {
        Id = comment.CommentId.Value.ToString();
        CreatedAt = comment.CreatedAt;
        UpdatedAt = comment.UpdatedAt;
        Body = comment.Body;
        Author = author;
    }
}

public record CommentsResponse(CommentResponse[] Comments);
