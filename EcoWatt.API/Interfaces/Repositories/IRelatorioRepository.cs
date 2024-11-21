using EcoWatt.API.Models;

namespace EcoWatt.API.Interfaces.Repositories
{
    public interface IRelatorioRepository
    {
        Task<IEnumerable<Relatorio>> GetAllAsync();
        Task<Relatorio> GetByIdAsync(int id);
        Task AddAsync(Relatorio relatorio);
        Task UpdateAsync(Relatorio relatorio);
        Task DeleteAsync(int id);
    }
}
