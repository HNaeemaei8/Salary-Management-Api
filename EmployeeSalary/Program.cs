using Application.Interfaces;
using EmployeeSalary.Application.Implementations;
using Domain.Interfaces;
using Infrastructure.Context;
using EmployeeSalary.Infrastucture.Repositories;
using Infrastructure.Common;
using Infrastructure.Infrastucture.Common;
using Infrastucture.Common.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ISalaryRepository, SalaryRepository>();
builder.Services.AddScoped<ISalaryService, SalaryService>();
builder.Services.AddScoped<IDataParserFactory, DataParserFactory>();
builder.Services.AddScoped<ISalaryCalculationService, SalaryCalculationService>();

// Parsers
builder.Services.AddScoped<IDataParser, JsonDataParser>();
builder.Services.AddScoped<IDataParser, XmlDataParser>();
builder.Services.AddScoped<IDataParser, CsvDataParser>();
builder.Services.AddScoped<IDataParser, CustomDataParser>();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Employee Salary API", Version = "v1" });

    
    var xmlFilename = $"EmployeeSalary.Servicehost.xml"; 
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.RoutePrefix = string.Empty;
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Employee Salary API v1");
    });
}
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();