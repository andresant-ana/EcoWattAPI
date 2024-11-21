using EcoWatt.API.DTOs;
using EcoWatt.API.Interfaces.Services;
using EcoWatt.API.Models;
using EcoWatt.API.Services;
using Microsoft.AspNetCore.Http;
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Relatorio>>> GetAll()
        {
            try
            {
                var relatorios = await _relatorioService.GetAllRelatoriosAsync();
                return Ok(relatorios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocorreu um erro interno.");
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Relatorio>> GetById(int id)
        {
            try
            {
                var relatorio = await _relatorioService.GetRelatorioByIdAsync(id);
                if (relatorio == null)
                    return NotFound();

                return Ok(relatorio);
            }
            catch (Exception ex)
            {
                // Log da exceção
                return StatusCode(500, "Ocorreu um erro interno.");
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Relatorio>> Create(RelatorioCreateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var relatorio = new Relatorio
                {
                    Nome = dto.Nome,
                    DataCriacao = DateTime.UtcNow
                };

                await _relatorioService.AddRelatorioAsync(relatorio);

                return CreatedAtAction(nameof(GetById), new { id = relatorio.Id }, relatorio);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocorreu um erro interno.");
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, RelatorioUpdateDto dto)
        {
            try
            {
                if (id != dto.Id)
                    return BadRequest("ID na rota não corresponde ao ID no corpo da requisição.");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var relatorioExistente = await _relatorioService.GetRelatorioByIdAsync(id);
                if (relatorioExistente == null)
                    return NotFound();

                relatorioExistente.Nome = dto.Nome;

                await _relatorioService.UpdateRelatorioAsync(relatorioExistente);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocorreu um erro interno.");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var relatorio = await _relatorioService.GetRelatorioByIdAsync(id);
                if (relatorio == null)
                    return NotFound();

                await _relatorioService.DeleteRelatorioAsync(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocorreu um erro interno.");
            }
        }
    }
}