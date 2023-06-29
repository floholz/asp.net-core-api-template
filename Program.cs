using System.Text;
using asp.net_core_api_template.Database;
using asp.net_core_api_template.Infrastructure;
using asp.net_core_api_template.Models.Authentication;
using asp.net_core_api_template.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<PostgresContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresDB"));
});

var tokenOptions = builder.Configuration.GetSection("token").Get<TokenOptions>();
// authentication with access token happens here
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(tokenOptions.Secret)),
        ValidIssuer = tokenOptions.Issuer,
        ValidAudience = tokenOptions.Audience,
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddScoped<UserService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
    c.SwaggerDoc("web-app", new OpenApiInfo { Title = "Web App API", Version = "v1" });
    c.SwaggerDoc("mobile-app", new OpenApiInfo { Title = "Mobile App API", Version = "v1" });
    
    c.DocInclusionPredicate((docName, apiDesc) =>
    {
        if (docName.Contains("web-app"))
        {
            return apiDesc.CustomAttributes()
                .OfType<IncludeInWebappApi>()
                .Any();
        }
        if (docName.Contains("mobile-app"))
        {
            return apiDesc.CustomAttributes()
                .OfType<IncludeInMobileAppApi>()
                .Any();
        }
        return true;
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(c => c.SerializeAsV2 = true);
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
        c.SwaggerEndpoint("/swagger/web-app/swagger.json", "Web App API v1");
        c.SwaggerEndpoint("/swagger/mobile-app/swagger.json", "Mobile App API v1");
    });
}

app.UseHttpsRedirection();
app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseAuthorization();
app.MapControllers();

app.Run();