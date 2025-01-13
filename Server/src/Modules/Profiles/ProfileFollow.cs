namespace BlogBackend.Modules.Profiles;

public class ProfileFollow
{
    public DateTime FollowedAt { get; init; } = DateTime.UtcNow;

    public ProfileId FollowerId { get; init; }
    public Profile Follower { get; init; }

    public ProfileId FollowingId { get; init; }
    public Profile Following { get; init; }

    private ProfileFollow() { }

    public ProfileFollow(Profile follower, Profile following)
    {
        Follower = follower;
        Following = following;
        FollowerId = follower.ProfileId;
        FollowingId = following.ProfileId;
    }
}
