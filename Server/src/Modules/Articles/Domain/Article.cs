using BlogBackend.Modules.Articles.Exceptions;
using BlogBackend.Modules.Articles.Utils;
using BlogBackend.Modules.Common;
using System.Text.Json.Serialization;

using static BlogBackend.Modules.Articles.Utils.StringToSlugConversions;
using static BlogBackend.Modules.Common.Utils.StringTransforms;

namespace BlogBackend.Modules.Articles.Domain;
public class Article
{
    [JsonIgnore]
    public ArticleId ArticleId { get; init; } = Guid.NewGuid();
    [JsonIgnore]
    public UserId AuthorId { get; init; }
    public string Slug { get; private set; }
    public string Title
    {
        get => _title;
        set
        {
            _title = value;
            Slug = _title
                .Transform(ToLowercase, ToAlphanumericOnly)
                .ToSlug(Hyphenate);
        }
    }
    private string _title;
    public string Description { 
        get => _description; 
        set
        {
            const int maxLength = 200;
            if(value.Length > maxLength)
            {
                throw new TextIsTooLongException(nameof(Description), maxLength);
            }
             _description = value;
        }
    }
    private string _description;
    public string Body { get; set; }
    public List<Tag> Tags { get; init; } = new List<Tag>();
    public List<Comment> Comments { get; init; } = new List<Comment>();
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public int FavoritesCount { get; private set; }
    public List<ArticleFavorited> ArticleFavoriteds { get; private set; } = new();

    public void Favorite(UserId userId)
    {
        var articleFavorited = new ArticleFavorited { UserId = userId, ArticleId = ArticleId };
        if (ArticleFavoriteds.Contains(articleFavorited))
            return;
        ArticleFavoriteds.Add(articleFavorited);
        FavoritesCount = ArticleFavoriteds.Count();
    }

    internal void Unfavorite(UserId userId)
    {
        ArticleFavoriteds.RemoveAll(x => x.UserId == userId);
        FavoritesCount = ArticleFavoriteds.Count();
    }
}

public record ArticleId(Guid Value)
{
    public static implicit operator Guid(ArticleId articleId) => articleId.Value;
    public static implicit operator ArticleId(Guid value) => new ArticleId(value);

}
