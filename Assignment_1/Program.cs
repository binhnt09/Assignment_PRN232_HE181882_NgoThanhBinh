using Assignment_1.Models;
using Assignment_1.Repositories;
using Assignment_1.service;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;

var builder = WebApplication.CreateBuilder(args);

// 1. CẤU HÌNH EDM MODEL CHO ODATA
var odataBuilder = new ODataConventionModelBuilder();
odataBuilder.EntitySet<SystemAccount>("SystemAccounts").EntityType.HasKey(s => s.AccountId);
odataBuilder.EntitySet<Category>("Categories").EntityType.HasKey(c => c.CategoryId);
odataBuilder.EntitySet<NewsArticle>("NewsArticles").EntityType.HasKey(n => n.NewsArticleId);
odataBuilder.EntitySet<Tag>("Tags").EntityType.HasKey(t => t.TagId);
// (Khai báo thêm NewsArticles và Categories ở đây nếu có)

// 2. ĐĂNG KÝ CONTROLLER + ODATA VÀO SERVICES
builder.Services.AddControllers()
    .AddOData(options =>
        options.Select().Filter().OrderBy().Expand().Count().SetMaxTop(100)
               // Đăng ký tiền tố "odata" nối với Model
               .AddRouteComponents("odata", odataBuilder.GetEdmModel())
    );

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 3. ĐĂNG KÝ DB CONTEXT VÀ DI (Dependency Injection)
builder.Services.AddDbContext<FunewsManagementContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// ĐĂNG KÝ CÁC SERVICES Ở ĐÂY (BẠN ĐANG THIẾU 2 DÒNG DƯỚI)
builder.Services.AddScoped<ISystemAccountService, SystemAccountService>();
builder.Services.AddScoped<ICategoryService, CategoryService>(); // Thêm dòng này
builder.Services.AddScoped<INewsArticleService, NewsArticleService>();
builder.Services.AddScoped<ITagService, TagService>();

var app = builder.Build();

// 4. THỨ TỰ MIDDLEWARE (BẮT BUỘC PHẢI THEO THỨ TỰ NÀY)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting(); // Bắt buộc phải có trước UseAuthorization

app.UseAuthorization();

app.MapControllers(); // Bắt buộc phải có để map các endpoint OData

app.Run();
