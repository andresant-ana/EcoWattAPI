using Microsoft.EntityFrameworkCore;
using EcoWatt.API.Models;

namespace EcoWatt.API.Data
{
    public class OracleDbContext : DbContext
    {
        public OracleDbContext(DbContextOptions<OracleDbContext> options) : base(options)
        {
        }

        // Definição dos DbSet para cada entidade
        public DbSet<ConsumoAgregado> ConsumosAgregados { get; set; }
        public DbSet<Relatorio> Relatorios { get; set; }
        public DbSet<Dispositivo> Dispositivos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração da relação entre ConsumoAgregado e Relatorio
            modelBuilder.Entity<ConsumoAgregado>()
                .HasOne(ca => ca.Relatorio)
                .WithMany(r => r.ConsumosAgregados)
                .HasForeignKey(ca => ca.RelatorioId)
                .OnDelete(DeleteBehavior.Cascade); // Define o comportamento de deleção

            // Configuração da relação entre ConsumoAgregado e Dispositivo (opcional)
            modelBuilder.Entity<ConsumoAgregado>()
                .HasOne(ca => ca.Dispositivo)
                .WithMany(d => d.ConsumosAgregados)
                .HasForeignKey(ca => ca.DispositivoId)
                .OnDelete(DeleteBehavior.SetNull); // Define o comportamento de deleção

            // Configurações adicionais, se necessário

            // Exemplo: Configuração de propriedades específicas
            modelBuilder.Entity<Relatorio>()
                .Property(r => r.Nome)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Dispositivo>()
                .Property(d => d.Nome)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<ConsumoAgregado>()
                .Property(ca => ca.Descricao)
                .HasMaxLength(255);
        }
    }
}