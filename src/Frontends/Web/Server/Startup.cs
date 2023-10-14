using BlazorHero.CleanArchitecture.Application.Extensions;
using BlazorHero.CleanArchitecture.Infrastructure.Extensions;
using BlazorHero.CleanArchitecture.Server.Extensions;
using BlazorHero.CleanArchitecture.Server.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System.IO;
using BlazorHero.CleanArchitecture.Server.Managers.Preferences;
using Microsoft.Extensions.Localization;

namespace BlazorHero.CleanArchitecture.Server;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    private readonly IConfiguration _configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddForwarding(_configuration);
        services.AddLocalization(options =>
        {
            options.ResourcesPath = "Resources";
        });
        services.AddCurrentUserService();
        services.AddSerialization();
        services.AddDatabase(_configuration);
        services.AddServerStorage(); 
        services.AddScoped<ServerPreferenceManager>();
        services.AddServerLocalization();
        services.AddIdentity();
        services.AddJwtAuthentication(services.GetApplicationSettings(_configuration));
        services.AddSignalR();
        services.AddApplicationLayer();
        services.AddApplicationServices();   
        services.AddSharedInfrastructure(_configuration);
        services.RegisterSwagger();
        services.AddInfrastructureMappings();         
        services.AddControllers().AddValidators();
        services.AddExtendedAttributesValidators();         
        services.AddRazorPages();            
        services.AddLazyCache();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IStringLocalizer<Startup> localizer)
    {
        app.UseForwarding(_configuration);
        app.UseExceptionHandling(env);
        app.UseHttpsRedirection();
        app.UseMiddleware<ErrorHandlerMiddleware>();
        app.UseBlazorFrameworkFiles();
        app.UseStaticFiles();
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Files")),
            RequestPath = new PathString("/Files")
        });
        app.UseRequestLocalizationByCulture();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();          
        app.UseEndpoints();
        app.ConfigureSwagger();
        app.UseCors("CorsPolicy");        
    }
}