using BlogBackend.Modules.Common;
using BlogBackend.Modules.Users;
using System.Text.Json.Serialization;

namespace BlogBackend.Modules.Profiles;

public class Profile
{
    [JsonIgnore]
    public ProfileId ProfileId { get; init; } = Guid.NewGuid();
    [JsonIgnore]
    public UserId UserId { get; init; }
    public string ProfileName { get; set; }
    public string? Bio { get; set; }
    public ProfileImage? Image { get; set; }

    [JsonIgnore]
    public List<ProfileFollow> Following { get; set; } = new();
    [JsonIgnore]
    public List<ProfileFollow> Followers { get; set; } = new();
    [JsonIgnore]
    public User User { get; set; }
}

public record ProfileImage(string Value);
