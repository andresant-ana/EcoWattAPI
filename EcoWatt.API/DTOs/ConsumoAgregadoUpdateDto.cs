namespace EcoWatt.API.DTOs
{
    public class ConsumoAgregadoUpdateDto
    {
        public int Id { get; set; }
        public int RelatorioId { get; set; }
        public int? DispositivoId { get; set; }
        public double Consumo { get; set; }
        public DateTime DataConsumo { get; set; }
        public string Descricao { get; set; }
    }
}