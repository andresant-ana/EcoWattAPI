using EcoWatt.API.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using EcoWatt.API.Interfaces.Repositories;
using EcoWatt.API.Interfaces.Services;

namespace EcoWatt.API.Services
{
    public class RelatorioService : IRelatorioService
    {
        private readonly IRelatorioRepository _relatorioRepository;

        public RelatorioService(IRelatorioRepository relatorioRepository)
        {
            _relatorioRepository = relatorioRepository;
        }

        public async Task<IEnumerable<Relatorio>> GetAllRelatoriosAsync()
        {
            return await _relatorioRepository.GetAllAsync();
        }

        public async Task<Relatorio> GetRelatorioByIdAsync(int id)
        {
            return await _relatorioRepository.GetByIdAsync(id);
        }

        public async Task AddRelatorioAsync(Relatorio relatorio)
        {
            relatorio.DataCriacao = DateTime.Now;
            await _relatorioRepository.AddAsync(relatorio);
        }

        public async Task UpdateRelatorioAsync(Relatorio relatorio)
        {
            await _relatorioRepository.UpdateAsync(relatorio);
        }

        public async Task DeleteRelatorioAsync(int id)
        {
            await _relatorioRepository.DeleteAsync(id);
        }
    }
}