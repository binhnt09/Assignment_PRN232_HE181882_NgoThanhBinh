using Assignment_2_BE.Models;
using Assignment_2_BE.Repositories;
using Assignment_2_BE.service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1. OData Setup
var odataBuilder = new ODataConventionModelBuilder();
odataBuilder.EntitySet<SystemAccount>("SystemAccounts").EntityType.HasKey(s => s.AccountId);
odataBuilder.EntitySet<Category>("Categories").EntityType.HasKey(c => c.CategoryId);
odataBuilder.EntitySet<NewsArticle>("NewsArticles").EntityType.HasKey(n => n.NewsArticleId);
odataBuilder.EntitySet<Tag>("Tags").EntityType.HasKey(t => t.TagId);

// 2. Controllers & OData
builder.Services.AddControllers()
    .AddOData(options =>
        options.Select().Filter().OrderBy().Expand().Count().SetMaxTop(100)
               .AddRouteComponents("odata", odataBuilder.GetEdmModel())
    );

// 3. DbContext
builder.Services.AddDbContext<FunewsManagementContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 4. AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// 5. Dependency Injection
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ISystemAccountService, SystemAccountService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
// builder.Services.AddScoped<INewsArticleService, NewsArticleService>();
// builder.Services.AddScoped<ITagService, TagService>();

// 6. JWT Authentication
var keyStr = builder.Configuration["Jwt:Key"] ?? "ThisIsASuperSecretKeyForJWTAuthenticationInFUNewsManagementSystem123!";
var key = Encoding.UTF8.GetBytes(keyStr);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

// 7. Swagger with JWT Support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Assignment_2_BE API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement{
    {
        new OpenApiSecurityScheme{
            Reference = new OpenApiReference{
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        },
        new string[]{}
    }});
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// IMPORTANT: Authentication must be before Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
