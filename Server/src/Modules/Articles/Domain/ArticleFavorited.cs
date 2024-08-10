using BlogBackend.Modules.Profiles;
using BlogBackend.Modules.Common;
using BlogBackend.Modules.Users;

namespace BlogBackend.Modules.Articles.Domain;

public class ArticleFavorited
{
    public ArticleId ArticleId { get; init; }
    public Article Article { get; init; }
    public UserId UserId { get; init; }
    public User User { get; init; }
}
