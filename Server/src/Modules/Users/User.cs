using BlogBackend.Modules.Common;

namespace BlogBackend.Modules.Users;

public class User
{
    public UserId UserId { get; init; } = Guid.NewGuid();
    public string Name { get; set; }
    public Email Email { get; set; }
    public string Password { get; set; }
}
