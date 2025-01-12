using BlogBackend.Modules.Common;
using BlogBackend.Modules.Users;
using System.Text.Json.Serialization;

namespace BlogBackend.Modules.Profiles;

public class Profile
{
    [JsonIgnore]
    public ProfileId ProfileId { get; init; } 
    [JsonIgnore]
    public UserId UserId { get; init; }
    public string ProfileName { get; set; }
    public string? Bio { get; set; }
    public ProfileImage? Image { get; set; }

    [JsonIgnore]
    public List<ProfileFollow> Following { get; } = [];
    [JsonIgnore]
    public List<ProfileFollow> Followers { get; } = [];
    [JsonIgnore]
    public User User { get; }

    public static Profile CreateNew(string profileName, UserId userId)
        => new(Guid.NewGuid(),userId, profileName, null, null);
    private Profile(ProfileId profileId, UserId userId, string profileName, string? bio, ProfileImage? image)
    {
        ProfileId = profileId;
        UserId = userId;
        ProfileName = profileName;
        Bio = bio;
        Image = image;
        User = default!;
    }
}

public record ProfileImage(string Value);
