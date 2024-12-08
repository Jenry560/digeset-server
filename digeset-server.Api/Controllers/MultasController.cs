
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using digeset_server.Core.entities;
using digeset_server.Infrastructure.Context;
using digeset_server.Core.dtos;
using AutoMapper;
using Solurecwebapi.Reponse;

namespace digeset_server.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MultasController : ControllerBase
    {

        private readonly digesetContext _context;
        private readonly IMapper _mapper;

        public MultasController(digesetContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("usuarioId/{usuarioId}")]
        public async Task<ActionResult<DataResponse<List<MultaDto>>>> GetMultaUsuarioId(int usuarioId)
        {
            try
            {
                var multas = await _context.Multa
                    .Where(x => usuarioId == 1 || x.Agente.UsuarioId == usuarioId)
                    .ToListAsync();

                if (!multas.Any())
                {
                    return NotFound(new DataResponse<List<MultaDto>>(false, "No se encontraron multas para el usuario especificado"));
                }

                var multaDtos = _mapper.Map<List<MultaDto>>(multas);
                return Ok(new DataResponse<List<MultaDto>>(true, "Multas obtenidas exitosamente", multaDtos));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new DataResponse<List<MultaDto>>(false, $"Error interno del servidor: {ex.Message}"));
            }
        }



        [HttpGet("agenteId/{agenteId}")]
        public async Task<ActionResult<DataResponse<List<MultaDto>>>> GetMultaAgenteId(int agenteId)
        {
            try
            {
                var multas = await _context.Multa
                    .Where(x => x.AgenteId == agenteId)
                    .ToListAsync();

                if (!multas.Any())
                {
                    return NotFound(new DataResponse<List<MultaDto>>(false, "No se encontraron multas para el agente especificado"));
                }

                var multaDtos = _mapper.Map<List<MultaDto>>(multas);
                return Ok(new DataResponse<List<MultaDto>>(true, "Multas obtenidas exitosamente", multaDtos));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new DataResponse<List<MultaDto>>(false, $"Error interno del servidor: {ex.Message}"));
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMulta(int id, [FromForm] MultaDto multaDto, IFormFile foto)
        {
            try
            {
                if (id != multaDto.MultaId)
                {
                    return BadRequest(new DataResponse<bool>(false, "El ID de la multa no coincide con el ID proporcionado", false));
                }

                if (multaDto == null)
                {
                    return BadRequest(new DataResponse<bool>(false, "Los datos del formulario son requeridos", false));
                }

                // Buscar la multa existente
                var existingMulta = await _context.Multa.FindAsync(id);
                if (existingMulta == null)
                {
                    return NotFound(new DataResponse<bool>(false, "La multa especificada no existe", false));
                }

                // Actualizar los datos de la multa existente
                _mapper.Map(multaDto, existingMulta);

                // Si se proporciona una nueva foto, reemplazarla
                if (foto != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await foto.CopyToAsync(memoryStream);
                        existingMulta.Foto = memoryStream.ToArray();
                    }
                }

                _context.Entry(existingMulta).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                    return Ok(new DataResponse<bool>(true, "Multa actualizada exitosamente", true));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MultaExists(id))
                    {
                        return NotFound(new DataResponse<bool>(false, "La multa especificada no existe", false));
                    }
                    throw;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new DataResponse<bool>(false, $"Error interno del servidor: {ex.Message}", false));
            }
        }



        [HttpPost]
        public async Task<ActionResult<DataResponse<MultaDto>>> PostMulta([FromForm] MultaDto multaDto, IFormFile foto)
        {
            try
            {
                if (foto == null)
                {
                    return BadRequest(
                        new DataResponse<MultaDto>(false, "La imagen es requerida.")
                    );
                }

                if (multaDto == null)
                {
                    return BadRequest(
                        new DataResponse<MultaDto>(false, "Los datos del formulario son requeridos.")
                    );
                }


                using (var memoryStream = new MemoryStream())
                {
                    await foto.CopyToAsync(memoryStream);
                    multaDto.Foto = memoryStream.ToArray();
                }

                var multa = _mapper.Map<Multa>(multaDto);

                _context.Multa.Add(multa);
                await _context.SaveChangesAsync();

                var createdMultaDto = _mapper.Map<MultaDto>(multa);

                return Ok(new DataResponse<MultaDto>(true, "Multa creada exitosamente", createdMultaDto));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new DataResponse<MultaDto>(false, $"Error interno del servidor: {ex.Message}"));
            }
        }




        [HttpPut("Estado/{multaId}/{estadoId}")]
        public async Task<ActionResult<DataResponse<string>>> PutEstado(int multaId, int estadoId)
        {
            try
            {
                var multa = await _context.Multa.FindAsync(multaId);

                if (multa == null)
                {
                    return NotFound(new DataResponse<string>(false, "La multa especificada no existe"));
                }

                multa.EstadoId = estadoId;

                _context.Multa.Update(multa);
                await _context.SaveChangesAsync();

                return Ok(new DataResponse<string>(true, "Estado de la multa actualizado exitosamente"));
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new DataResponse<string>(false, "Error al actualizar el estado de la multa"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new DataResponse<string>(false, $"Error interno del servidor: {ex.Message}"));
            }
        }

        [HttpGet("reporteIngreso/{usuarioId}/{mes}/{ano}")]
        public async Task<ActionResult<DataResponse<List<ReporteIngresosDto>>>> GetReporteIngresos(int usuarioId, string mes, string ano)
        {
            try
            {
                int mesInt = int.Parse(mes);
                int anoInt = int.Parse(ano);

                var reporteIngresos = await _context.Multa
                    .Where(x => x.Agente.UsuarioId == usuarioId &&
                                x.FechaCreacion.HasValue &&
                                x.FechaCreacion.Value.Month == mesInt &&
                                x.FechaCreacion.Value.Year == anoInt)
                    .GroupBy(m => m.ConceptoId)
                    .Select(r => new ReporteIngresosDto
                    {
                        ConceptoId = r.Key,
                        TipoMulta = r.FirstOrDefault()!.Concepto.Descripcion,
                        CantidadMulta = r.Count(),
                        TotalIngresos = r.Sum(r => r.Concepto.Monto)
                    })
                    .ToListAsync();

                if (!reporteIngresos.Any())
                {
                    return NotFound(new DataResponse<List<ReporteIngresosDto>>(false, "No se encontraron ingresos para el usuario y período especificados"));
                }

                return Ok(new DataResponse<List<ReporteIngresosDto>>(true, "Reporte de ingresos obtenido exitosamente", reporteIngresos));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new DataResponse<List<ReporteIngresosDto>>(false, $"Error interno del servidor: {ex.Message}"));
            }
        }


        [HttpGet("comisionesPorMes/{agenteId}/{mes}")]
        public async Task<ActionResult<DataResponse<ReporteComisionDto>>> GetComisionesPorMes(int agenteId, string mes)
        {
            try
            {
                int mesInt = int.Parse(mes);

                var totalComision = await _context.Multa
                    .Where(x => x.AgenteId == agenteId &&
                                x.EstadoId == 1 &&
                                x.FechaCreacion.HasValue &&
                                x.FechaCreacion.Value.Month == mesInt)
                    .SumAsync(r => r.Concepto.Monto) * 0.10;

                var ultimasMultas = await _context.Multa
                    .Where(x => x.AgenteId == agenteId && 
                                x.FechaCreacion.HasValue &&
                                x.FechaCreacion.Value.Month == mesInt)
                    .OrderByDescending(m => m.FechaCreacion)
                    .Take(5)
                    .ToListAsync();

                var reporte = new ReporteComisionDto
                {
                    TotalComision = totalComision,
                    Multas = _mapper.Map<List<MultaDto>>(ultimasMultas)
                };

                return Ok(new DataResponse<ReporteComisionDto>(true, "Reporte de comisiones obtenido exitosamente", reporte));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new DataResponse<ReporteComisionDto>(false, $"Error interno del servidor: {ex.Message}"));
            }
        }


        private bool MultaExists(int id)
        {
            return _context.Multa.Any(e => e.MultaId == id);
        }
    }
}
