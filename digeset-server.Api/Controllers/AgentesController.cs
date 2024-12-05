using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using digeset_server.Core.entities;
using digeset_server.Infrastructure.Context;
using AutoMapper;
using digeset_server.Core.dtos;
using Solurecwebapi.Reponse;

namespace digeset_server.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentesController : ControllerBase
    {
        private readonly digesetContext _context;
        private readonly IMapper _mapper;

        public AgentesController(digesetContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }



        // GET: api/Agentes
        [HttpGet("usuarioId")]
        public async Task<ActionResult<DataResponse<List<AgenteDto>>>> GetAgentes(int usuarioId)
        {
            try
            {
                var agentes = await _context.Agentes.Where(x => x.UsuarioId == usuarioId).ToListAsync();
                var agentesDto = _mapper.Map<List<AgenteDto>>(agentes);

                if (!agentesDto.Any())
                {
                    return NotFound(new DataResponse<List<AgenteDto>>(false, "No se encontraron agentes para el usuario especificado"));
                }

                return Ok(new DataResponse<List<AgenteDto>>(true, "Datos obtenidos exitosamente", agentesDto));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new DataResponse<List<AgenteDto>>(false, $"Error interno del servidor: {ex.Message}"));
            }
        }



        [HttpPut("{id}")]
        public async Task<ActionResult<DataResponse<string>>> PutAgente(int id, [FromBody] Agente agente)
        {
            if (id != agente.AgenteId)
            {
                return BadRequest(new DataResponse<string>(false, "El ID proporcionado no coincide con el agente"));
            }

            _context.Entry(agente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new DataResponse<string>(true, "Agente actualizado exitosamente"));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AgenteExists(id))
                {
                    return NotFound(new DataResponse<string>(false, "El agente no existe"));
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new DataResponse<string>(false, "Error de concurrencia al actualizar el agente"));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new DataResponse<string>(false, $"Error interno del servidor: {ex.Message}"));
            }
        }


        [HttpPut("Estado/{id}/{estado}")]
        public async Task<ActionResult<DataResponse<string>>> EstadoAgente(int id, bool estado)
        {
            try
            {
                var agente = await _context.Agentes.FindAsync(id);
                if (agente == null)
                {
                    return NotFound(new DataResponse<string>(false, "El agente no existe"));
                }

                agente.Estado = estado;
                _context.Entry(agente).State = EntityState.Modified;

                await _context.SaveChangesAsync();

                return Ok(new DataResponse<string>(true, "Estado del agente actualizado exitosamente"));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AgenteExists(id))
                {
                    return NotFound(new DataResponse<string>(false, "El agente no existe"));
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new DataResponse<string>(false, "Error de concurrencia al actualizar el estado del agente"));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new DataResponse<string>(false, $"Error interno del servidor: {ex.Message}"));
            }
        }


        // POST: api/Agentes
        [HttpPost]
        public async Task<ActionResult<DataResponse<AgenteDto>>> PostAgente([FromBody] AgenteDto agenteDto)
        {
            try
            {
                var agente = _mapper.Map<Agente>(agenteDto);

                // Agregar lógica adicional si es necesario (ej. validaciones específicas)
                _context.Agentes.Add(agente);
                await _context.SaveChangesAsync();

                var createdAgenteDto = _mapper.Map<AgenteDto>(agente);

                return CreatedAtAction(
                    nameof(GetAgentes),
                    new { id = createdAgenteDto.AgenteId },
                    new DataResponse<AgenteDto>(true, "Agente creado exitosamente", createdAgenteDto)
                );
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new DataResponse<AgenteDto>(false, $"Error interno del servidor: {ex.Message}"));
            }
        }


        private bool AgenteExists(int id)
        {
            return _context.Agentes.Any(e => e.AgenteId == id);
        }
    }
}
