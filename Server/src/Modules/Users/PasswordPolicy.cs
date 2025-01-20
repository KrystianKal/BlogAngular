namespace BlogBackend.Modules.Users;

public static class PasswordPolicyModule
{
    public delegate bool PasswordPolicy(string input);

    public static bool ExecuteFor(this PasswordPolicy policy, string input)
        => policy.Invoke(input);
    public static bool ExecuteFor(this PasswordPolicy[] policies, string input)
        => policies.All().Invoke(input);

    private static PasswordPolicy All(this PasswordPolicy[] policies) => policies switch
    {
        [] => NoConstraint,
        [var single] => single,
        [var head, .. var tail] => head.And(tail.All())
    };

    private static PasswordPolicy And(this PasswordPolicy left, PasswordPolicy right)
        => input => left(input) && right(input);

    private static PasswordPolicy NoConstraint => _ => true;
    public static PasswordPolicy AtLeast(int lenght) => input => input.Length >= lenght;
    public static PasswordPolicy ContainsUpperLetter => input => input.Any(char.IsUpper);
    public static PasswordPolicy ContainsLowerLetter => input => input.Any(char.IsLower);
    
}