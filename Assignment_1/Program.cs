using Assignment_1.Models;
using Assignment_1.Repositories;
using Assignment_1.service;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var odataBuilder = new ODataConventionModelBuilder();
odataBuilder.EntitySet<SystemAccount>("SystemAccounts").EntityType.HasKey(s => s.AccountId);
IEdmModel GetEdmModel()
{
    odataBuilder.EntitySet<NewsArticle>("NewsArticles").EntityType.HasKey(n => n.NewsArticleId);
    odataBuilder.EntitySet<Category>("Categories").EntityType.HasKey(c => c.CategoryId);

    return odataBuilder.GetEdmModel();
}

// Đăng ký DbContext với MS SQL Server
builder.Services.AddDbContext<FunewsManagementContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddControllers().AddOData(options =>
{
    options.Select().Filter().OrderBy().Expand().Count().SetMaxTop(100)
           .AddRouteComponents("odata", odataBuilder.GetEdmModel());
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Đăng ký Generic Repository
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// Đăng ký các Services
builder.Services.AddScoped<ISystemAccountService, SystemAccountService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<INewsArticleService, NewsArticleService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
