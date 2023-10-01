using HH_ASP_APP;
using HH_ASP_APP.Hubs;
using HH_ASP_APP.Interfaces;
using HH_ASP_APP.Middleware;
using HH_ASP_APP.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();

builder.Services.Configure<RabbitMqConfiguration>(conf =>
{
    conf.HostName = "localhost";
    conf.Username = "guest";
    conf.Password = "guest";
});

builder.Services.AddSingleton<IRabbitMqService, RabbitMqService>();
builder.Services.AddScoped<IOperationServices, OperationServices>();

builder.Services.AddScoped<ErrorHandlingMiddleware>();

var app = builder.Build();

app.Lifetime.ApplicationStarted.Register(() =>
{  
    var rabbit = new RabbitMQInitializer(app.Services);
    rabbit.Initialize();  
});

app.UseMiddleware<ErrorHandlingMiddleware>();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<RabbitMQHub>("/rabbitmqHub");

app.Run();
