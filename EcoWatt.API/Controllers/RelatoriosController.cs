using EcoWatt.API.Models;
using EcoWatt.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcoWatt.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RelatoriosController : ControllerBase
    {
        private readonly IRelatorioService _relatorioService;

        public RelatoriosController(IRelatorioService relatorioService)
        {
            _relatorioService = relatorioService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Relatorio>>> GetAll()
        {
            var relatorios = await _relatorioService.GetAllRelatoriosAsync();
            return Ok(relatorios);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Relatorio>> GetById(int id)
        {
            var relatorio = await _relatorioService.GetRelatorioByIdAsync(id);
            if (relatorio == null)
                return NotFound();
            return Ok(relatorio);
        }

        [HttpPost]
        public async Task<ActionResult> Create(Relatorio relatorio)
        {
            await _relatorioService.AddRelatorioAsync(relatorio);
            return CreatedAtAction(nameof(GetById), new { id = relatorio.Id }, relatorio);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, Relatorio relatorio)
        {
            if (id != relatorio.Id)
                return BadRequest();

            await _relatorioService.UpdateRelatorioAsync(relatorio);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var relatorio = await _relatorioService.GetRelatorioByIdAsync(id);
            if (relatorio == null)
                return NotFound();

            await _relatorioService.DeleteRelatorioAsync(id);
            return NoContent();
        }
    }
}