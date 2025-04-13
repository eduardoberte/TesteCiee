using System.ComponentModel.DataAnnotations;

namespace ApiAnimais.Models;

public class CuidadoModel
{
    public Guid Id { get; init; } = Guid.NewGuid();

    [Required]
    public string Nome { get; set; }

    public string? Descricao { get; set; }

    public string? Frequencia { get; set; }

    public ICollection<AnimalCuidado> Animais { get; set; } = new List<AnimalCuidado>();
}
