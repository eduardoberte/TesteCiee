using Microsoft.EntityFrameworkCore;
using ApiAnimais.Data;
using ApiAnimais.Models;

namespace ApiAnimais.Routes;

public static class CuidadoRoutes
{
    public static void MapCuidadoRoutes(this WebApplication app)
    {
        var group = app.MapGroup("/cuidados");

        //associação de cuidado ao animal 
        group.MapPost("/associar", async (AssociarCuidadoRequest req, AnimalContext context) =>
        {
            var animal = await context.Animais.FindAsync(req.AnimalId);
            var cuidado = await context.Cuidados.FindAsync(req.CuidadoId);

            if (animal == null || cuidado == null)
                return Results.NotFound("Animal ou cuidado não encontrado.");

            var jaExiste = await context.Set<AnimalCuidado>()
                .AnyAsync(ac => ac.AnimalId == req.AnimalId && ac.CuidadoId == req.CuidadoId);

            if (jaExiste)
                return Results.Conflict("Associação já existe.");

            var associacao = new AnimalCuidado
            {
                AnimalId = req.AnimalId,
                CuidadoId = req.CuidadoId
            };

            context.Set<AnimalCuidado>().Add(associacao);
            await context.SaveChangesAsync();

            return Results.Ok("Cuidado associado com sucesso!");
        });

        // Cadastrar cuidado
        group.MapPost("/", async (CuidadoRequest req, AnimalContext context) =>
        {
            var cuidado = new CuidadoModel
            {
                Nome = req.Nome,
                Descricao = req.Descricao,
                Frequencia = req.Frequencia
            };

            await context.Cuidados.AddAsync(cuidado);
            await context.SaveChangesAsync();

            return Results.Created($"/cuidados/{cuidado.Id}", cuidado);
        });

        // Listar todos os cuidados
        group.MapGet("/", async (AnimalContext context) =>
        {
            var cuidados = await context.Cuidados.ToListAsync();
            return Results.Ok(cuidados);
        });

        // Listar cuidados por animalId
        group.MapGet("/animal/{id:guid}", async (Guid id, AnimalContext context) =>
        {
            var animal = await context.Animais
                .Include(a => a.Cuidados)
                .ThenInclude(ac => ac.Cuidado)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (animal == null)
                return Results.NotFound();

            var cuidados = animal.Cuidados.Select(ac => new {
                nome = ac.Cuidado.Nome,
                descricao = ac.Cuidado.Descricao,
                frequencia = ac.Cuidado.Frequencia
            });

            return Results.Ok(cuidados);
        });

        // Atualizar cuidado
        group.MapPut("/{id:guid}", async (Guid id, CuidadoRequest req, AnimalContext context) =>
        {
            var cuidado = await context.Cuidados.FindAsync(id);
            if (cuidado == null) return Results.NotFound();

            cuidado.Nome = req.Nome;
            cuidado.Descricao = req.Descricao;
            cuidado.Frequencia = req.Frequencia;

            await context.SaveChangesAsync();
            return Results.Ok(cuidado);
        });

        group.MapDelete("/{id:guid}", async (Guid id, AnimalContext context) =>
        {
            var cuidado = await context.Cuidados
                .Include(c => c.Animais)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cuidado == null) return Results.NotFound();

            context.Set<AnimalCuidado>().RemoveRange(cuidado.Animais);

            context.Cuidados.Remove(cuidado);
            await context.SaveChangesAsync();
            return Results.NoContent();
        });
    }

    public record AssociarCuidadoRequest(Guid AnimalId, Guid CuidadoId);
}
