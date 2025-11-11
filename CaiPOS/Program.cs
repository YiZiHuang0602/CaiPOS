using CaiPOS.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument();
builder.Services.AddDbContext<DbConn>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("CaiPOSContext")));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();      // 生成 Swagger JSON 文件 (預設路徑: /swagger/v1/swagger.json)
    app.UseSwaggerUi();    // 啟用 Swagger UI (預設路徑: /swagger)
    app.UseReDoc();        // 啟用 ReDoc (可選)
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
