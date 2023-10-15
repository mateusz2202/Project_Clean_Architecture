using Identity.API;

var builder = WebApplication.CreateBuilder(args);

var app = builder
       .ConfigureServices()
       .ConfigurePipeline()
       .Initialize(builder.Configuration);


app.Run();