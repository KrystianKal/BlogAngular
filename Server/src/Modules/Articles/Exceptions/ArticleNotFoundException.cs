using BlogBackend.Modules.Common.Exceptions;
using System.Net;

namespace BlogBackend.Modules.Articles.Exceptions;

public class ArticleNotFoundException(string Slug)
    : ApiException(HttpStatusCode.NotFound, new { Article = $"Article with slug: {Slug} not found" });
