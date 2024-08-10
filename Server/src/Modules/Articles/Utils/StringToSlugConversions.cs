namespace BlogBackend.Modules.Articles.Utils;
using static BlogBackend.Modules.Common.Utils.StringTransforms;

public delegate string StringToSlug(string s);
public static class StringToSlugConversions{
    public static string ToSlug(this string s, StringToSlug conversion)
        => conversion(s);
    public static StringToSlug Hyphenate =>
        s => new(string.Join('-',s.SeparateOnWhitespace()));
}