using BlogBackend;
using BlogBackend.Modules.Common.Database;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o =>
    {
        o.LoginPath = null;
        o.LogoutPath = null;
        o.Events.OnRedirectToLogin = ctx =>
        {
            ctx.Response.StatusCode = 401;
            return Task.CompletedTask;
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddCors(c => c.AddPolicy("myPolicy", b =>
    b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

builder.Services.AddPostgres<BlogDbContext>(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSwaggerGen(o =>
{
    o.CustomSchemaIds(x => x.FullName);
});
builder.Services.AddCore();

var app = builder.Build();

app.Services.EnsureDatabaseCreated();

app.UsePathBase(new PathString("/api"));


if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI();
    app.UseSwagger();
}


app.UseExceptionHandler(new ExceptionHandlerOptions()
{
    ExceptionHandlingPath = "/api"
});
app.UseStaticFiles();

app.UseHttpsRedirection();


app.MapControllers();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(_ => { });

app.UseSpa(x => x.UseProxyToSpaDevelopmentServer(
    builder.Configuration.GetSection("FrontendUrl").Value ?? "http://localhost:4200"));


app.Run();

public partial class Program { }
