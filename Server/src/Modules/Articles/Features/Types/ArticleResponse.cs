using System.Runtime.CompilerServices;
using BlogBackend.Modules.Articles.Domain;

namespace BlogBackend.Modules.Articles.Features.Types;

public record ArticleResponse (
    string Slug,
    string Title,
    string Description,
    string Body,
    string[] TagList,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    bool? Favorited,
    int FavoritesCount,
    Author Author )
{
    public static ArticleResponse Create(Article article, Author author, bool? favorited = null) =>
        new(
            Slug: article.Slug,
            Title: article.Title,
            Description: article.Description,
            Body: article.Body,
            TagList: article.Tags.Select(x => x.Name).ToArray(),
            CreatedAt: article.CreatedAt,
            UpdatedAt: article.UpdatedAt,
            FavoritesCount: article.FavoritesCount,
            Author: author,
            Favorited: favorited
        );
}

public record ArticlesResponse(ArticleResponse[] Articles, int ArticleCount);
