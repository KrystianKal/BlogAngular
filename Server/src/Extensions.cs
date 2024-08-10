using BlogBackend.Modules.Articles.Utils;
using BlogBackend.Modules.Common;
using BlogBackend.Modules.Common.Exceptions;
using BlogBackend.Modules.Common.PipelineBehaviours;
using BlogBackend.Modules.Profiles.Utils;
using FluentValidation;
using System.Reflection;

namespace BlogBackend;

public static class Extensions
{
    public static void AddCore(this IServiceCollection services)
    {
        ValidatorOptions.Global.DefaultClassLevelCascadeMode = CascadeMode.Stop;
        services.AddExceptionHandler<ApiExceptionHandler>();
        services.AddScoped<IUserAccessor, UserAccessor>();
        services.AddScoped<IAuthorService, AuthorService>();
        services.AddScoped<IImageService, ImageService>();
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(o =>
        {
            o.RegisterServicesFromAssembly(typeof(Program).Assembly);
            o.AddOpenBehavior(typeof(ValidationPipelineBehaviour<,>));
        }
        );
    }

}
