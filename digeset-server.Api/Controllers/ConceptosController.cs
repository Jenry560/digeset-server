
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
        public async Task<ActionResult<DataResponse<List<ConceptoDto>>>> GetConceptos()
        {
            try
            {
                List<Concepto> conceptos = await _context.Conceptos.ToListAsync();
                var conceptoDtos = _mapper.Map<List<ConceptoDto>>(conceptos);
                return Ok(new DataResponse<List<ConceptoDto>>(true, "Datos obtenidos exitosamente", conceptoDtos));
            }
            catch (Exception ex)
            {
                return new DataResponse<List<ConceptoDto>>(false, $"Error interno del servidor: {ex.Message}");
            }
        }


        [HttpPut]
        public async Task<ActionResult<DataResponse<ConceptoDto>>> PutConcepto(ConceptoDto concepto)
        {
            try
            {
                _context.Entry(_mapper.Map<Concepto>(concepto)).State = EntityState.Modified;

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConceptoExists(concepto.ConceptoId))
                {

                    return BadRequest(new DataResponse<ConceptoDto>(false, "Concepto no encontrado"));
                }
                else
                {
                    return BadRequest(new DataResponse<ConceptoDto>(false, "Error interno del servidor"));
                    throw;
                }
            }

            return Ok(new DataResponse<ConceptoDto>(true, "Concepto actualizado exitosamente", concepto));
        }

        // POST: api/Conceptos
        [HttpPost]
        public async Task<ActionResult<DataResponse<ConceptoDto>>> PostConcepto([FromBody] ConceptoDto conceptoDto)
        {
            try
            {
                if (conceptoDto == null)
                {
                    return BadRequest(new DataResponse<ConceptoDto>(false, "El objeto ConceptoDto no puede ser nulo"));
                }

                var concepto = _mapper.Map<Concepto>(conceptoDto);
                _context.Conceptos.Add(concepto);
                await _context.SaveChangesAsync();

                var createdDto = _mapper.Map<ConceptoDto>(concepto);

                return CreatedAtAction(nameof(GetConceptos), new { id = concepto.ConceptoId },
                    new DataResponse<ConceptoDto>(true, "Concepto creado exitosamente", createdDto));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new DataResponse<ConceptoDto>(false, $"Error interno del servidor: {ex.Message}"));
            }
        }


        // DELETE: api/Conceptos/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<DataResponse<string>>> DeleteConcepto(int id)
        {
            try
            {
                var concepto = await _context.Conceptos.FindAsync(id);
                if (concepto == null)
                {
                    return NotFound(new DataResponse<string>(false, "El concepto con el ID especificado no existe"));
                }

                _context.Conceptos.Remove(concepto);
                await _context.SaveChangesAsync();

                return Ok(new DataResponse<string>(true, "Concepto eliminado exitosamente"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new DataResponse<string>(false, $"Error interno del servidor: {ex.Message}"));
            }
        }

        private bool ConceptoExists(int id)
        {
            return _context.Conceptos.Any(e => e.ConceptoId == id);
        }
    }
}
