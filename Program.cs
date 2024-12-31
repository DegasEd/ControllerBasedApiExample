using ControllerBasedApiSwagger;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

// Trecho específico para esta linha abaixo do código -> Importante!
var builder = WebApplication.CreateBuilder(args);

// Adiciona via injeção de dependência um contexto de banco de dados em memória
builder.Services.AddDbContext<MovieDb>(opt => 
    opt.UseInMemoryDatabase("MovieDbList"));

// Realiza a configuração da página do Swagger
// Fica o convite para mergulhar e entender melhor a respeito
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Movie API",
        Description = "API for managing a list of movies and their active status.",
        TermsOfService = new Uri("https://example.com/terms")
    });
});

builder.Services.AddControllers();

var app = builder.Build();

// Middleware Swagger
app.UseSwagger();

// Middleware Swagger UI
app.UseSwaggerUI();

// Realiza redirecionamento para utilização de HTTPS, mesmo em ambiente de desenvolvimento
app.UseHttpsRedirection();

// Realiza o mapeamento das controllers / endpoints adicionadas na forma controller-based.
// Fica o convite para explorar melhor este middleware
app.MapControllers();

// Executa efetivamente a aplicação
app.Run();
