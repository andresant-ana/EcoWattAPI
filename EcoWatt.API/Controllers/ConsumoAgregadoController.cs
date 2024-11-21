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
    public class ConsumoAgregadoController : ControllerBase
    {
        private readonly IConsumoAgregadoService _consumoAgregadoService;

        public ConsumoAgregadoController(IConsumoAgregadoService consumoAgregadoService)
        {
            _consumoAgregadoService = consumoAgregadoService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ConsumoAgregado>>> GetAll()
        {
            try
            {
                var consumos = await _consumoAgregadoService.GetAllConsumosAsync();
                return Ok(consumos);
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
        public async Task<ActionResult<ConsumoAgregado>> GetById(int id)
        {
            try
            {
                var consumo = await _consumoAgregadoService.GetConsumoByIdAsync(id);
                if (consumo == null)
                    return NotFound();

                return Ok(consumo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocorreu um erro interno.");
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ConsumoAgregado>> Create(ConsumoAgregadoCreateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (!await _consumoAgregadoService.DispositivoExistsAsync(dto.DispositivoId))
                    return BadRequest($"Dispositivo com Id {dto.DispositivoId} não encontrado.");

                var consumoAgregado = new ConsumoAgregado
                {
                    RelatorioId = dto.RelatorioId,
                    DispositivoId = dto.DispositivoId,
                    Consumo = dto.Consumo,
                    DataConsumo = dto.DataConsumo,
                    Descricao = dto.Descricao
                };

                await _consumoAgregadoService.AddConsumoAsync(consumoAgregado);

                return CreatedAtAction(nameof(GetById), new { id = consumoAgregado.Id }, consumoAgregado);
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
        public async Task<IActionResult> Update(int id, ConsumoAgregadoUpdateDto dto)
        {
            try
            {
                if (id != dto.Id)
                    return BadRequest("ID na rota não corresponde ao ID no corpo da requisição.");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (!await _consumoAgregadoService.DispositivoExistsAsync(dto.DispositivoId))
                    return BadRequest($"Dispositivo com Id {dto.DispositivoId} não encontrado.");

                var consumoExistente = await _consumoAgregadoService.GetConsumoByIdAsync(id);
                if (consumoExistente == null)
                    return NotFound();

                consumoExistente.RelatorioId = dto.RelatorioId;
                consumoExistente.DispositivoId = dto.DispositivoId;
                consumoExistente.Consumo = dto.Consumo;
                consumoExistente.DataConsumo = dto.DataConsumo;
                consumoExistente.Descricao = dto.Descricao;

                await _consumoAgregadoService.UpdateConsumoAsync(consumoExistente);

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
                var consumo = await _consumoAgregadoService.GetConsumoByIdAsync(id);
                if (consumo == null)
                    return NotFound();

                await _consumoAgregadoService.DeleteConsumoAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocorreu um erro interno.");
            }
        }
    }
}