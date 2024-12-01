
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
    public class ConceptosController : ControllerBase
    {
        private readonly digesetContext _context;
        private readonly IMapper _mapper;

        public ConceptosController(digesetContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        // GET: api/Conceptos
        [HttpGet]
        public async Task<ActionResult<ConceptoDto>> GetConceptos()
        {
            List<Concepto> conceptos = await _context.Conceptos.ToListAsync();
            var conceptoDtos = _mapper.Map<ConceptoDto>(conceptos);
            return Ok(conceptoDtos);
        }

  
        [HttpPut]
        public async Task<IActionResult> PutConcepto(ConceptoDto concepto)
        {
         

            _context.Entry(_mapper.Map<Concepto>(concepto)).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConceptoExists(concepto.ConceptoId))
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

        // POST: api/Conceptos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Concepto>> PostConcepto(ConceptoDto concepto)
        {
            _context.Conceptos.Add(_mapper.Map<Concepto>(concepto));
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetConcepto", new { id = concepto.ConceptoId }, concepto);
        }

        // DELETE: api/Conceptos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConcepto(int id)
        {
            var concepto = await _context.Conceptos.FindAsync(id);
            if (concepto == null)
            {
                return NotFound();
            }

            _context.Conceptos.Remove(concepto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ConceptoExists(int id)
        {
            return _context.Conceptos.Any(e => e.ConceptoId == id);
        }
    }
}
