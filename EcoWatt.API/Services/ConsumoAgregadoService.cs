using EcoWatt.API.Data;
using EcoWatt.API.Models;
using EcoWatt.API.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoWatt.API.Services
{
    public class ConsumoAgregadoService : IConsumoAgregadoService
    {
        private readonly IConsumoAgregadoRepository _consumoAgregadoRepository;
        private readonly OracleDbContext _context;

        public ConsumoAgregadoService(IConsumoAgregadoRepository consumoAgregadoRepository, OracleDbContext context)
        {
            _consumoAgregadoRepository = consumoAgregadoRepository;
            _context = context;
        }

        public async Task<IEnumerable<ConsumoAgregado>> GetAllConsumosAsync()
        {
            return await _consumoAgregadoRepository.GetAllAsync();
        }

        public async Task<ConsumoAgregado> GetConsumoByIdAsync(int id)
        {
            return await _consumoAgregadoRepository.GetByIdAsync(id);
        }

        public async Task AddConsumoAsync(ConsumoAgregado consumoAgregado)
        {
            await _consumoAgregadoRepository.AddAsync(consumoAgregado);
        }

        public async Task UpdateConsumoAsync(ConsumoAgregado consumoAgregado)
        {
            await _consumoAgregadoRepository.UpdateAsync(consumoAgregado);
        }

        public async Task DeleteConsumoAsync(int id)
        {
            await _consumoAgregadoRepository.DeleteAsync(id);
        }

        public async Task<bool> DispositivoExistsAsync(int? dispositivoId)
        {
            if (!dispositivoId.HasValue)
                return true; // Se não há DispositivoId, considera válido

            return await _context.Dispositivos.AnyAsync(d => d.Id == dispositivoId.Value);
        }
    }
}