using BlogBackend.Modules.Common;
using System.Text.Json.Serialization;

namespace BlogBackend.Modules.Articles.Domain;

public class Comment
{
    public CommentId CommentId { get; init; } = Guid.NewGuid();
    public UserId AuthorId { get; init; }
    public string Body { get; set; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    [JsonIgnore]
    public ArticleId ArticleId { get; set; }
}

public record CommentId(Guid Value)
{
    public static implicit operator CommentId(Guid value) => new CommentId(value);
    public static implicit operator Guid(CommentId commentId) => commentId.Value;
};