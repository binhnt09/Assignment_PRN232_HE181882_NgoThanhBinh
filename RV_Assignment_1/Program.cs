using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;
using RV_Assignment_1.Models;
using Microsoft.OData.Edm;
using RV_Assignment_1.Repositories;

var builder = WebApplication.CreateBuilder(args);

// 1. Cấu hình EF Core SQL Server kết nối database
builder.Services.AddDbContext<CampusBulletinTestContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//var odataBuilder = new ODataConventionModelBuilder();
//odataBuilder.EntitySet<MemberAccount>("MemberAccounts").EntityType.HasKey(s => s.AccountId);
//odataBuilder.EntitySet<BulletinCategory>("BulletinCategorys").EntityType.HasKey(c => c.CategoryId);
//odataBuilder.EntitySet<BulletinPost>("BulletinPosts").EntityType.HasKey(n => n.PostId);
// 2. Đăng ký các Repository vào DI Container
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IBulletinPostRepository, BulletinPostRepository>();

static IEdmModel GetEdmModel()
{
    var builder = new ODataConventionModelBuilder();
    builder.EntitySet<BulletinPost>("BulletinPosts");
    builder.EntitySet<BulletinCategory>("BulletinCategories");
    return builder.GetEdmModel();
}

// 3. Khai báo Controllers tích hợp hỗ trợ bộ lọc OData
builder.Services.AddControllers().AddOData(options =>
    options.Select().Filter().OrderBy().Expand().Count().SetMaxTop(100)
           .AddRouteComponents("odata", GetEdmModel()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Tự động khởi tạo database và cập nhật Migrations dữ liệu nếu cần
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CampusBulletinTestContext>();
    db.Database.EnsureCreated(); // Tạo DB và chèn sẵn dữ liệu seed ngay lập tức khi chạy ứng dụng
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();
