using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RestaueantAPI.Models;
using RestaueantAPI.ModelsPostGres;
using RestaueantAPI.UATModels;
using Npgsql;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<MomkenContext>(
	OptionsBuilder => { OptionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("DBUAT")); }
);

builder.Services.AddDbContext<AdminToolContext>(
    OptionsBuilder => { OptionsBuilder.UseNpgsql(builder.Configuration.GetConnectionString("DBPostg")); }
    );

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
