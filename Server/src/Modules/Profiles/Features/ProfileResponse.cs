namespace BlogBackend.Modules.Profiles.Features;

public record ProfileResponse(string Name, string Bio, string Image, bool? Following = null)
{
    public static ProfileResponse From(Profile profile, bool? Following = null)
    {
        return new ProfileResponse(profile.ProfileName, profile.Bio, profile.Image?.Value, Following);
    }
};
