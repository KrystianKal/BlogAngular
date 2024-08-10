namespace BlogBackend.Modules.Profiles;

public record ProfileId(Guid Value)
{
    public static implicit operator Guid(ProfileId value) => value.Value;
    public static implicit operator ProfileId(Guid value) => new ProfileId(value);
}
