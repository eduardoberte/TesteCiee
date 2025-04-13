using System.ComponentModel.DataAnnotations;

namespace ApiAnimais.Models;

public class CuidadoRequest
{
    [Required]
    public string Nome { get; set; }

    public string? Descricao { get; set; }

    public string? Frequencia { get; set; }
}
