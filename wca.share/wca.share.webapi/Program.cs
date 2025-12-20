using wca.share.infrastructure;
using wca.share.application;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureApplication(builder.Configuration);
builder.Services.ConfigureInfraStructure(builder.Configuration);

var issuer = builder.Configuration["TokenConfigurations:Issuer"];
var audienceSection = builder.Configuration.GetSection("TokenConfigurations:Audience");
List<string>? audiences = audienceSection.Get<List<string>>();
var secret = builder.Configuration["TokenConfigurations:Secret"].ToString();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, // Deve validar quem emitiu
            ValidateAudience = true, // Deve validar para quem se destina
            ValidateLifetime = true, // Deve validar a data de expiração
            ValidateIssuerSigningKey = true, // Deve validar a chave secreta

            // As mesmas configurações da API Compras:
            ValidIssuer = issuer,
            ValidAudiences = audiences,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanAccessSystem", policy =>
        policy.RequireClaim("sistema", "share"));
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "WCA Share API",
        Description = "Controle de requisições do rh"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Entre com o token Bearer JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement{
    {
        new OpenApiSecurityScheme {
            Reference = new OpenApiReference {
            Id = "Bearer",
            Type = ReferenceType.SecurityScheme
            }
        }, new List<string>()
    }
});
    
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpContext();

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();



app.UseCors(option => option.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.MapControllers();

app.Run();
