using System.ComponentModel.DataAnnotations;

namespace ApiAnimais.Models;

public class AnimalModel
{
    public Guid Id { get; init; } = Guid.NewGuid();

    [Required]
    public string Nome { get; set; }

    public string? Descricao { get; set; }

    [Required]
    public DateTime DataNascimento { get; set; }

    public string? Especie { get; set; }

    public string? Habitat { get; set; }

    public string? PaisOrigem { get; set; }

    public ICollection<AnimalCuidado> Cuidados { get; set; } = new List<AnimalCuidado>();
}
