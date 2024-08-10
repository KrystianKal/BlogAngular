namespace BlogBackend.Modules.Common.Utils;
public delegate string TransformString(string s); 

public static class StringTransforms{
    public static TransformString ToLowercase => 
        s => s.ToLower(); 

    public static TransformString ToAlphanumericOnly =>
        s =>{
            var validChars = s.Where( c => char.IsLetterOrDigit(c)).ToArray();
            return new string(validChars);
        };
    public static string[] SeparateOnWhitespace(this string s)
        =>  s.Split([' ', '\t'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

    // e.g. string.Transform(ToLowercase, ToAlphanumericOnly)
    public static string Transform(this string s, params TransformString[] transforms)
        => transforms.Aggregate(s, (current, transform) => transform(current));
}

