using ReceitasMarombaApi.Interfaces;
using ReceitasMarombaApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using ReceitasMarombaApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IReceitaRepository, ReceitaRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/v1/receitas", ([FromServices] IReceitaRepository receitaRepository) => 
{
    return receitaRepository.GetAll();
});

app.MapGet("/v1/receitas/{id}", ([FromServices] IReceitaRepository receitaRepository, [FromRoute] int id) => 
{
    return receitaRepository.GetOne(id);
});

app.MapPost("/v1/receitas", ([FromServices] IReceitaRepository receitaRepository, [FromBody] ReceitaModel data) => 
{
    var result = receitaRepository.Create(data);

    if (result)
        return Results.Ok("Receita criada com sucesso!");
    else
        return Results.BadRequest("Não foi possível inserir receita.");
});

app.MapDelete("/v1/receitas/{id}", ([FromServices] IReceitaRepository receitaRepository, [FromRoute] int id) => 
{
    var result = receitaRepository.Delete(id);

    if (result)
        return Results.Ok("Receita foi excluída com sucesso!");
    else
        return Results.BadRequest($"Não foi possível excluir receita com Id = {id}.");
});

app.MapPut("/v1/receitas", ([FromServices] IReceitaRepository receitaRepository, [FromBody] ReceitaModel data) => 
{
    var result = receitaRepository.Update(data.Id, data);

    if (result)
        return Results.Ok("Receita atualizada com sucesso!");
    else
        return Results.BadRequest("Erro ao atualizar receita.");
});

app.MapGet("/v1/receitas/{id}/ingredientes", ([FromRoute] int id) => {
    return $"Ingredientes da receita com Id = {id}";
});

app.Run();
