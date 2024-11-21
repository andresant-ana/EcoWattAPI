using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcoWatt.API.Models
{
    public class ConsumoAgregado
    {
        public int Id { get; set; }
        public int RelatorioId { get; set; }
        public Relatorio Relatorio { get; set; }

        public int? DispositivoId { get; set; }
        public Dispositivo? Dispositivo { get; set; }

        public double Consumo { get; set; }
        public DateTime DataConsumo { get; set; }

        [Column("DESCRICAO")]
        public string Descricao { get; set; }
    }
}