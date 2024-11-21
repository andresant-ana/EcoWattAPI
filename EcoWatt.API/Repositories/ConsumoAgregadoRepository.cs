using EcoWatt.API.Data;
using EcoWatt.API.Interfaces.Repositories;
using EcoWatt.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoWatt.API.Repositories
{
    public class ConsumoAgregadoRepository : IConsumoAgregadoRepository
    {
        private readonly OracleDbContext _context;

        public ConsumoAgregadoRepository(OracleDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ConsumoAgregado>> GetAllAsync()
        {
            return await _context.ConsumosAgregados
                .Include(ca => ca.Relatorio)
                .Include(ca => ca.Dispositivo)
                .ToListAsync();
        }

        public async Task<ConsumoAgregado> GetByIdAsync(int id)
        {
            return await _context.ConsumosAgregados
                .Include(ca => ca.Relatorio)
                .Include(ca => ca.Dispositivo)
                .FirstOrDefaultAsync(ca => ca.Id == id);
        }

        public async Task AddAsync(ConsumoAgregado consumoAgregado)
        {
            await _context.ConsumosAgregados.AddAsync(consumoAgregado);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ConsumoAgregado consumoAgregado)
        {
            _context.ConsumosAgregados.Update(consumoAgregado);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var consumoAgregado = await _context.ConsumosAgregados.FindAsync(id);
            if (consumoAgregado != null)
            {
                _context.ConsumosAgregados.Remove(consumoAgregado);
                await _context.SaveChangesAsync();
            }
        }
    }
}