using api.AutoMapper;
using api.Extensions;
using api.IServices;
using api.Models;
using api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddDbContext<BankingDbContext>(options => options.UseSqlite("Data " +
    "source=banking.db"));

builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();

var app = builder.Build();

// Apply migrations and seed the database
if(app.Environment.IsDevelopment())
{
    app.ApplyMigrations();
    app.SeedDataBase();
}

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
