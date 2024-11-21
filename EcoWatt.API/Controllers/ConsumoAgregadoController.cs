using EcoWatt.API.DTOs;
using EcoWatt.API.Models;
using EcoWatt.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public async Task<ActionResult<IEnumerable<ConsumoAgregado>>> GetAll()
        {
            var consumos = await _consumoAgregadoService.GetAllConsumosAsync();
            return Ok(consumos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ConsumoAgregado>> GetById(int id)
        {
            var consumo = await _consumoAgregadoService.GetConsumoByIdAsync(id);
            if (consumo == null)
                return NotFound();
            return Ok(consumo);
        }

        [HttpPost]
        public async Task<ActionResult> Create(ConsumoAgregadoCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool dispositivoExiste = await _consumoAgregadoService.DispositivoExistsAsync(dto.DispositivoId);
            if (!dispositivoExiste)
            {
                return BadRequest($"Dispositivo com Id {dto.DispositivoId.Value} não encontrado.");
            }

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

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, ConsumoAgregadoUpdateDto dto)
        {
            if (id != dto.Id)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool dispositivoExiste = await _consumoAgregadoService.DispositivoExistsAsync(dto.DispositivoId);
            if (!dispositivoExiste)
            {
                return BadRequest($"Dispositivo com Id {dto.DispositivoId.Value} não encontrado.");
            }

            var consumoAgregado = new ConsumoAgregado
            {
                Id = dto.Id,
                RelatorioId = dto.RelatorioId,
                DispositivoId = dto.DispositivoId,
                Consumo = dto.Consumo,
                DataConsumo = dto.DataConsumo,
                Descricao = dto.Descricao
            };

            await _consumoAgregadoService.UpdateConsumoAsync(consumoAgregado);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var consumo = await _consumoAgregadoService.GetConsumoByIdAsync(id);
            if (consumo == null)
                return NotFound();

            await _consumoAgregadoService.DeleteConsumoAsync(id);
            return NoContent();
        }
    }
}