namespace BlogBackend.Modules.Articles.Features.Types;

public record Author(string ProfileName, string UserName, string? Bio = null,string? Image = null, bool? Following = null);