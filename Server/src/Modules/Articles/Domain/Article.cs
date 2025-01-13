using BlogBackend.Modules.Articles.Exceptions;
using BlogBackend.Modules.Articles.Utils;
using BlogBackend.Modules.Common;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using static BlogBackend.Modules.Articles.Utils.StringToSlugConversions;
using static BlogBackend.Modules.Common.Utils.StringTransforms;

namespace BlogBackend.Modules.Articles.Domain;
public class Article
{
    [JsonIgnore]
    public ArticleId ArticleId { get; init; }
    [JsonIgnore]
    public UserId AuthorId { get; init; }
    public string Slug { get; private set; } = null!;

    public string Title { get;
        set
        {
            field = value;
            Slug = value
                .Transform(ToLowercase, ToAlphanumericOnly)
                .ToSlug(Hyphenate);
        }
    }
    public string Description { get;
        set
        {
            const int maxLength = 200;
            field = value.Length > maxLength 
                ? throw new TextIsTooLongException(nameof(Description), maxLength)
                : value;
        }
   }

    public string Body { get; set; }
    public List<Tag> Tags { get; init; }
    public List<Comment> Comments { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; set; }
    public int FavoritesCount { get; private set; }
    public List<ArticleFavorited> ArticleFavoriteds { get; } = [];

    public void Favorite(UserId userId)
    {
        var articleFavorited = new ArticleFavorited { UserId = userId, ArticleId = ArticleId };
        if (ArticleFavoriteds.Contains(articleFavorited))
            return;
        ArticleFavoriteds.Add(articleFavorited);
        FavoritesCount = ArticleFavoriteds.Count;
    }

    internal void Unfavorite(UserId userId)
    {
        ArticleFavoriteds.RemoveAll(x => x.UserId == userId);
        FavoritesCount = ArticleFavoriteds.Count;
    }

    public static Article CreateNew(
        UserId authorId,
        string title,
        string description,
        string body,
        List<Tag>? tags = null) =>
        new(Guid.NewGuid(), authorId, title, description, body, tags ?? []);

    private Article(ArticleId articleId,
        UserId authorId,
        string title,
        string description,
        string body,
        List<Tag> tags)
    :this (articleId,authorId,title, description, body)
    {
        Tags = tags;
    }
    
    private Article(ArticleId articleId,        //EF Core
        UserId authorId,
        string title,
        string description,
        string body)
    {
        
        ArticleId = articleId;
        AuthorId = authorId;
        Title = title;
        Description = description;
        Body = body;
        Tags = default!;
        Comments = [];
        CreatedAt = DateTime.UtcNow;
        FavoritesCount = 0;
    }
}

public record ArticleId(Guid Value)
{
    public static implicit operator Guid(ArticleId articleId) => articleId.Value;
    public static implicit operator ArticleId(Guid value) => new(value);
}
