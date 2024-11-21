using EcoWatt.API.Models;

namespace EcoWatt.API.Interfaces.Services
{
    public interface IRelatorioService
    {
        Task<IEnumerable<Relatorio>> GetAllRelatoriosAsync();
        Task<Relatorio> GetRelatorioByIdAsync(int id);
        Task AddRelatorioAsync(Relatorio relatorio);
        Task UpdateRelatorioAsync(Relatorio relatorio);
        Task DeleteRelatorioAsync(int id);
    }
}
