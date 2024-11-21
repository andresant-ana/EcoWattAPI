using EcoWatt.API.Data;
using EcoWatt.API.Models;
using Microsoft.EntityFrameworkCore;

namespace EcoWatt.API.Repositories
{
    public class RelatorioRepository : IRelatorioRepository
    {
        private readonly OracleDbContext _context;

        public RelatorioRepository(OracleDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Relatorio>> GetAllAsync()
        {
            return await _context.Relatorios.ToListAsync();
        }

        public async Task<Relatorio> GetByIdAsync(int id)
        {
            return await _context.Relatorios.FindAsync(id);
        }

        public async Task AddAsync(Relatorio relatorio)
        {
            await _context.Relatorios.AddAsync(relatorio);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Relatorio relatorio)
        {
            _context.Entry(relatorio).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var relatorio = await _context.Relatorios.FindAsync(id);
            if (relatorio != null)
            {
                _context.Relatorios.Remove(relatorio);
                await _context.SaveChangesAsync();
            }
        }
    }
}
