using API_Identity;
using API_Identity.Data;
using API_Identity.Entities;
using API_Identity.Interfaces;
using API_Identity.Middleware;
using API_Identity.Models.Validator;
using API_Identity.Models;
using API_Identity.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddFluentValidationAutoValidation();

builder.Services.Configure<RouteOptions>(o =>
{
    o.LowercaseUrls = true;
    o.LowercaseQueryStrings = true;
});

builder.Services.AddDbContext<IndentityDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnectionString")));

// Add services to the container.
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserContextService, UserContextService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<ErrorHandlingMiddleware>();


builder.Services.AddScoped<IValidator<LoginDTO>, LoginDTOValidator>();
builder.Services.AddScoped<IValidator<RegisterUserDTO>, RegisterUserDTOValidator>();
builder.Services.AddScoped<IValidator<UpdatePasswordDTO>, UpdatePasswordDTOValidator>();


var authenticationSetting = new AuthenticationSettings();
builder.Configuration.GetSection(nameof(AuthenticationSettings)).Bind(authenticationSetting);
builder.Services.AddSingleton(authenticationSetting);

builder.Services.Configure<AuthenticationSettings>(conf => builder.Configuration.GetSection(nameof(AuthenticationSettings)).Bind(conf));

builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = "Bearer";
    option.DefaultScheme = "Bearer";
    option.DefaultChallengeScheme = "Bearer";
}).AddJwtBearer(cfg =>
{
    cfg.RequireHttpsMetadata = false;
    cfg.SaveToken = true;
    cfg.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration[$"{nameof(AuthenticationSettings)}:JwtIssuer"],
        ValidAudience = builder.Configuration[$"{nameof(AuthenticationSettings)}:JwtIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration[$"{nameof(AuthenticationSettings)}:JwtKey"] ?? string.Empty))
    };
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddCors();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
     {
         {
               new OpenApiSecurityScheme
                 {
                     Reference = new OpenApiReference
                     {
                         Type = ReferenceType.SecurityScheme,
                         Id = "Bearer"
                     }
                 },
                 Array.Empty<string>()
         }
     });
    options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
});

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
