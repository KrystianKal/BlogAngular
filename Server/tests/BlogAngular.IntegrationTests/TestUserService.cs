using BlogBackend.Modules.Users;

namespace BlogAngular.IntegrationTests;

public class TestUserService
{
    private User _user;
    public User User { 
        get => _user;
        set
        {
            _user = value;
            _user.Password = "test";
        }
    }
}
