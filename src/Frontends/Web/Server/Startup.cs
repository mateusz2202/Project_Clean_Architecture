using BlazorHero.CleanArchitecture.Application.Extensions;
using BlazorHero.CleanArchitecture.Infrastructure.Extensions;
using BlazorHero.CleanArchitecture.Server.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using BlazorHero.CleanArchitecture.Server.Managers.Preferences;

namespace BlazorHero.CleanArchitecture.Server;

public class Startup
{
   
    public void ConfigureServices(IServiceCollection services)
    {     
        services.AddLocalization(options =>
        {
            options.ResourcesPath = "Resources";
        });
        services.AddCurrentUserService();
        services.AddSerialization();      
        services.AddServerStorage(); 
        services.AddScoped<ServerPreferenceManager>();
        services.AddServerLocalization();    
        services.AddSignalR();
        services.AddApplicationLayer();     
        services.AddInfrastructureMappings();     
        services.AddControllers();
        services.AddRazorPages();            
        services.AddLazyCache();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {       
        app.UseExceptionHandling(env);
        app.UseHttpsRedirection();     
        app.UseBlazorFrameworkFiles();
        app.UseStaticFiles();       
        app.UseRequestLocalizationByCulture();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();          
        app.UseEndpoints();      
        app.UseCors("CorsPolicy");        
    }
}