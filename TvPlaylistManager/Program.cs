using Microsoft.EntityFrameworkCore;
using TvPlaylistManager.Infrastructure;
using TvPlaylistManager.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

ConfigureServices(builder.Services, builder.Configuration);
builder.Services.ConfigureDependencies();

var app = builder.Build();

ConfigureMiddleware(app);

app.Run();

void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    services.AddControllersWithViews()
        .AddRazorOptions(options =>
        {
            options.ViewLocationFormats.Clear();
            options.ViewLocationFormats.Add("/web/views/{1}/{0}.cshtml");
            options.ViewLocationFormats.Add("/web/views/Shared/{0}.cshtml");
        });

    services.AddDbContext<AppDbContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

    services.AddCors(options =>
    {
        options.AddPolicy("AllowAll",
            builder => builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
    });

    services.AddHttpClient("EpgClient", client =>
    {
        client.DefaultRequestHeaders.Add("Accept", "application/xml");
        client.Timeout = TimeSpan.FromSeconds(30);
    });
}

void ConfigureMiddleware(WebApplication app)
{
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }
    else
    {
        app.UseDeveloperExceptionPage();
    }

    app.UseHttpsRedirection();
    app.UseRouting();
    app.UseAuthorization();
    app.MapStaticAssets();
    app.UseCors("AllowAll");

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
        .WithStaticAssets();

}
