namespace BlogBackend.Modules.Common;

public record Email(string Value)
{
    public static implicit operator string(Email email) => email.Value;
    public static implicit operator Email(string value) => new Email(value);
};
