using static BlogBackend.Modules.Articles.Utils.StringToSlugConversions;

namespace BlogAngular.UnitTests.Articles.Utils;

public class StringToSlugConversionsTests
{
    [Fact]
    public void ConvertStringToHyphanatedSlug()
    {
        string input = "one two three";
        string expected = "one-two-three";

        string res = input.ToSlug(Hyphenate);
        Assert.Equal(expected, res);
    }
}