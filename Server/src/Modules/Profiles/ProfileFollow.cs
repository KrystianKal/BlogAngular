namespace BlogBackend.Modules.Profiles;

public class ProfileFollow
{
    public DateTime FollowedAt { get; init; } = DateTime.UtcNow;

    public ProfileId FollowerId { get; set; }
    public Profile Follower { get; set; }

    public ProfileId FollowingId { get; set; }
    public Profile Following { get; set; }

    private ProfileFollow() { }

    public ProfileFollow(Profile follower, Profile following)
    {
        Follower = follower;
        Following = following;
        FollowerId = follower.ProfileId;
        FollowingId = following.ProfileId;
    }
}
