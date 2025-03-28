using Agenda.Application;
using Agenda.Infrastructure;
using Agenda.CrossCutting;
using Agenda.Domain.Repositories;
using Agenda.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Agenda.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<AgendaDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IContatoRepository, ContatoRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuração do CORS para permitir tanto o Vercel quanto o localhost
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVercelAndLocalhost", policy =>
    {
        policy.WithOrigins("https://agenda-app-beige.vercel.app", "http://localhost:8080")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();


app.UseCors("AllowVercelAndLocalhost");

app.MapControllers();

app.Run();
