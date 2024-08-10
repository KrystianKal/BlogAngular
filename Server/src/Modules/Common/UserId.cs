namespace BlogBackend.Modules.Common;

public record UserId(Guid Value)
{
    public static implicit operator Guid(UserId value) => value.Value;
    public static implicit operator UserId(Guid value) => new UserId(value);
    public static UserId Parse(string value) => new UserId(Guid.Parse(value));
}
