using BlazorHero.CleanArchitecture.Application;
using BlazorHero.CleanArchitecture.Server.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorHero.CleanArchitecture.Server;

public class Startup
{
   
    public void ConfigureServices(IServiceCollection services)
    {             
        services.AddSignalR();
        services.AddApplicationLayer();    
        services.AddControllers();
        services.AddRazorPages();            
        services.AddLazyCache();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {       
        app.UseExceptionHandling(env);
        app.UseHttpsRedirection();     
        app.UseBlazorFrameworkFiles();    
        app.UseRequestLocalizationByCulture();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();          
        app.UseEndpoints();      
        app.UseCors("CorsPolicy");        
    }
}