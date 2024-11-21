using System;
using System.Collections.Generic;

namespace EcoWatt.API.Models
{
    public class Relatorio
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public DateTime DataCriacao { get; set; }

        public ICollection<ConsumoAgregado> ConsumosAgregados { get; set; } = new List<ConsumoAgregado>();
    }
}