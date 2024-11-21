using EcoWatt.API.Models;

namespace EcoWatt.API.Interfaces.Repositories
{
    public interface IConsumoAgregadoRepository
    {
        Task<IEnumerable<ConsumoAgregado>> GetAllAsync();
        Task<ConsumoAgregado> GetByIdAsync(int id);
        Task AddAsync(ConsumoAgregado consumoAgregado);
        Task UpdateAsync(ConsumoAgregado consumoAgregado);
        Task DeleteAsync(int id);
    }
}
