using BibliotecaAPI.Repositories;
using BibliotecaAPI.Repositories.Implementations;
using BibliotecaAPI.Repositories.Interfaces;
using BibliotecaAPI.Services.Implementations;
using BibliotecaAPI.Services.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configura��o do AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

//// Configura��o do Banco de Dados
//builder.Services.AddDbContext<BibliotecaContext>(options =>
//{
//    options.UseSqlServer(builder.Configuration.GetConnectionString("BibliotecaConnection"));
//    options.EnableDetailedErrors(); // mensagens de erro mais detalhadas
//    options.EnableSensitiveDataLogging(); // ver valores reais nos logs
//    options.LogTo(Console.WriteLine, LogLevel.Information); // Log das consultas SQL
//});

// Configura��o conex�o SQL
builder.Services.AddScoped<IDbConnection>(provider =>
    new SqlConnection(builder.Configuration.GetConnectionString("BibliotecaConnection")));

//// Configura��o do Repositories (EF Core)
//builder.Services.AddScoped<ILivroRepository, LivroRepository>();
//builder.Services.AddScoped<IAutorRepository, AutorRepository>();
//builder.Services.AddScoped<IAssuntoRepository, AssuntoRepository>();

// Configura��o do Repositories (ADO.NET/Dapper)
builder.Services.AddScoped<ILivroRepository, LivroAdoRepository>();
builder.Services.AddScoped<IAutorRepository, AutorAdoRepository>();
builder.Services.AddScoped<IAssuntoRepository, AssuntoAdoRepository>();

// Configura��o do Services
builder.Services.AddScoped<ILivroService, LivroService>();
builder.Services.AddScoped<IAutorService, AutorService>();
builder.Services.AddScoped<IAssuntoService, AssuntoService>();

// Configura��o do CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularOrigins",
    builder =>
    {
        builder.WithOrigins("http://localhost:4200", "https://localhost:4200")
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAngularOrigins");

app.UseAuthorization();

app.MapControllers();

app.Run();