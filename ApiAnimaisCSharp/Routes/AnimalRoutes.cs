using Microsoft.EntityFrameworkCore;
using ApiAnimais.Data;
using ApiAnimais.Models;

namespace ApiAnimais.Routes;

public static class AnimalRoutes
{
    public static void MapAnimalRoutes(this WebApplication app)
    {
        var group = app.MapGroup("/animais");

        // Cadastrar Animal
        group.MapPost("/", async (AnimalRequest req, AnimalContext context) =>
        {
            var animal = new AnimalModel
            {
                Nome = req.Nome,
                Descricao = req.Descricao,
                DataNascimento = req.DataNascimento,
                Especie = req.Especie,
                Habitat = req.Habitat,
                PaisOrigem = req.PaisOrigem
            };

            if (req.CuidadosIds != null)
            {
                var cuidados = await context.Cuidados
                    .Where(c => req.CuidadosIds.Contains(c.Id))
                    .ToListAsync();

                foreach (var c in cuidados)
                {
                    animal.Cuidados.Add(new AnimalCuidado
                    {
                        Animal = animal,
                        Cuidado = c
                    });
                }
            }

            await context.AddAsync(animal);
            await context.SaveChangesAsync();
            return Results.Created($"/animais/{animal.Id}", animal);
        });

        // Listar Animais
        group.MapGet("/", async (AnimalContext context) =>
        {
            var animais = await context.Animais
                .Include(a => a.Cuidados)
                .ThenInclude(ac => ac.Cuidado)
                .ToListAsync();

            return Results.Ok(animais);
        });

        // Atualizar Animal
        group.MapPut("/{id:guid}", async (Guid id, AnimalRequest req, AnimalContext context) =>
        {
            var animal = await context.Animais
                .Include(a => a.Cuidados)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (animal == null) return Results.NotFound();

            animal.Nome = req.Nome;
            animal.Descricao = req.Descricao;
            animal.DataNascimento = req.DataNascimento;
            animal.Especie = req.Especie;
            animal.Habitat = req.Habitat;
            animal.PaisOrigem = req.PaisOrigem;

            animal.Cuidados.Clear();

            if (req.CuidadosIds != null)
            {
                var cuidados = await context.Cuidados
                    .Where(c => req.CuidadosIds.Contains(c.Id))
                    .ToListAsync();

                foreach (var c in cuidados)
                {
                    animal.Cuidados.Add(new AnimalCuidado
                    {
                        AnimalId = animal.Id,
                        CuidadoId = c.Id
                    });
                }
            }

            await context.SaveChangesAsync();
            return Results.Ok(animal);
        });

        // Remover Animal
        group.MapDelete("/{id:guid}", async (Guid id, AnimalContext context) =>
        {
            var animal = await context.Animais.FindAsync(id);
            if (animal == null) return Results.NotFound();

            context.Animais.Remove(animal);
            await context.SaveChangesAsync();
            return Results.NoContent();
        });
    }
}
