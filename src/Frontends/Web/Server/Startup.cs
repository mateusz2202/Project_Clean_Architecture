using BlazorHero.CleanArchitecture.Application.Extensions;
using BlazorHero.CleanArchitecture.Infrastructure.Extensions;
using BlazorHero.CleanArchitecture.Server.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
        services.AddServerStorage(); 
        services.AddScoped<ServerPreferenceManager>();
        services.AddServerLocalization();     
        services.AddJwtAuthentication(services.GetApplicationSettings(_configuration));
        services.AddSignalR();
        services.AddApplicationLayer();   
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
        app.UseBlazorFrameworkFiles();
        app.UseStaticFiles();       
        app.UseRequestLocalizationByCulture();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();          
        app.UseEndpoints();
        app.ConfigureSwagger();
        app.UseCors("CorsPolicy");        
    }
}