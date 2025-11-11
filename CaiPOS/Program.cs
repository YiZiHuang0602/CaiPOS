using CaiPOS.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<CaiPOSContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CaiPOSContext") ?? throw new InvalidOperationException("Connection string 'CaiPOSContext' not found.")));

// Add services to the container.

builder.Services.AddControllers(); // 添加控制器服務
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(); 
builder.Services.AddEndpointsApiExplorer(); // 添加端點 API 探索服務

builder.Services.AddSwaggerGen(); // 添加 Swagger 生成服務


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
