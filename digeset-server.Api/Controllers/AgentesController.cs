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
        public async Task<ActionResult<AgenteDto>> GetAgentes(int usuarioId)
        {
            var agentes = await _context.Agentes.Where(x => x.UsuarioId == usuarioId).ToListAsync();
            return _mapper.Map<AgenteDto>(agentes);
        }


        // PUT: api/Agentes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAgente(int id, Agente agente)
        {
            if (id != agente.AgenteId)
            {
                return BadRequest();
            }

            _context.Entry(agente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AgenteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }



        // PUT: api/Agentes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("Estado/{id}/{estado}")]
        public async Task<IActionResult> EstadoAgente(int id, bool estado)
        {
           

            var agente = await _context.Agentes.FindAsync(id);
            if (agente == null)
            {
                return BadRequest();
            }

            agente.Estado = estado;
            _context.Agentes.Update(agente);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AgenteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Agentes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Agente>> PostAgente(AgenteDto agente)
        {
            _context.Agentes.Add(_mapper.Map<Agente>(agente));
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAgente", new { id = agente.AgenteId }, agente);
        }
       
        private bool AgenteExists(int id)
        {
            return _context.Agentes.Any(e => e.AgenteId == id);
        }
    }
}
