namespace BlogAngular.UnitTests.Common.Utils;

using static BlogBackend.Modules.Common.Utils.StringTransforms;


public class StringTransformsTests
{
    [Theory]
    [InlineData("abc","abc")]
    [InlineData(" 1 2 c ","12c")]
    [InlineData("h*(dd)","hdd")]
    public void ToAlphanumericOnly_Returns_AlphanumericOnly(string input, string expected)
    {
        string result = input.Transform(ToAlphanumericOnly);
        Assert.Equal(expected, result);

    }
    [Fact]
    public void SeparateOnWhitespace_Returns_EmptyArray()
    {
        string input = "   \t   ";
        string[] expected = new string[] { };
        
        string[] result = input.SeparateOnWhitespace();
        
        Assert.Equal(expected, result);
    }
}