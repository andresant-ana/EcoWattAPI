using EcoWatt.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoWatt.API.Services
{
    public interface IConsumoAgregadoService
    {
        Task<IEnumerable<ConsumoAgregado>> GetAllConsumosAsync();
        Task<ConsumoAgregado> GetConsumoByIdAsync(int id);
        Task AddConsumoAsync(ConsumoAgregado consumoAgregado);
        Task UpdateConsumoAsync(ConsumoAgregado consumoAgregado);
        Task DeleteConsumoAsync(int id);
        Task<bool> DispositivoExistsAsync(int? dispositivoId);
    }
}