using System.Text.Json.Serialization;

namespace BlogBackend.Modules.Articles.Domain;

public class Tag(string name)
{
    public TagId TagId { get; init; } = Guid.NewGuid();
    public string Name { get; set; } = name;

    [JsonIgnore]
    public List<Article> Articles { get; init; } = [];
}

public record TagId(Guid Value)
{
    public static implicit operator Guid(TagId tagId) => tagId.Value;
    public static implicit operator TagId(Guid value) => new TagId(value);

}
