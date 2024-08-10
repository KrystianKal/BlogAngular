using BlogBackend.Modules.Articles.Domain;

namespace BlogBackend.Modules.Articles.Features.Types;

public record ArticleResponse
{
    public string Slug { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
    public string Body { get; init; }
    public string[] TagList { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
    public bool? Favorited { get; init; }
    public int FavoritesCount { get; init; }
    public Author Author { get; init; }
    public ArticleResponse(Article article, Author author ,bool? favorited = null)
    {
        Slug = article.Slug;
        Title = article.Title;
        Description = article.Description;
        Body = article.Body;
        TagList = article.Tags.Select(x => x.Name).ToArray();
        CreatedAt = article.CreatedAt;
        UpdatedAt = article.UpdatedAt;
        FavoritesCount = article.FavoritesCount;
        Author = author;
        if (favorited is not null) Favorited = favorited;
    }
}

public record ArticlesResponse(ArticleResponse[] Articles, int ArticleCount);
