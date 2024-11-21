using System.Collections.Generic;

namespace EcoWatt.API.Models
{
    public class Dispositivo
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Tipo { get; set; }

        public ICollection<ConsumoAgregado> ConsumosAgregados { get; set; } = new List<ConsumoAgregado>();
    }
}