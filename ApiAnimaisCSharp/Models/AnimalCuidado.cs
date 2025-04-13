namespace ApiAnimais.Models;

public class AnimalCuidado
{
    public Guid AnimalId { get; set; }

    public AnimalModel Animal { get; set; }

    public Guid CuidadoId { get; set; }

    public CuidadoModel Cuidado { get; set; }


}
