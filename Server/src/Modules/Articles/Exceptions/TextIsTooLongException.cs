using BlogBackend.Modules.Common.Exceptions;
using System.Net;

namespace BlogBackend.Modules.Articles.Exceptions;

public class TextIsTooLongException(string propertyName, int charLimit)
    : ApiException(
        HttpStatusCode.BadRequest,
         new Dictionary<string,string>{ { propertyName , $"Character limit of {charLimit} exceeded" } }
        );
