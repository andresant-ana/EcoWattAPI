namespace EcoWatt.API.DTOs
{
    public class ConsumoAgregadoCreateDto
    {
        public int RelatorioId { get; set; }
        public int? DispositivoId { get; set; }
        public double Consumo { get; set; }
        public DateTime DataConsumo { get; set; }
        public string Descricao { get; set; }
    }
}