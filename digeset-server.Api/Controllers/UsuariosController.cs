
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
    public class UsuariosController : ControllerBase
    {
        private readonly digesetContext _context;
        private readonly IMapper _mapper;

        public UsuariosController(digesetContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<DataResponse<List<UsuarioDto>>>> GetUsuarios()
        {

            try
            {
                var usuarios = await _context.Usuarios.ToListAsync();

                return Ok(new DataResponse<List<UsuarioDto>>(true, "Datos obtenidos exitosamente", _mapper.Map<List<UsuarioDto>>(usuarios)));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new DataResponse<List<UsuarioDto>>(false, $"Error interno del servidor: {ex.Message}"));
            }

        }


        [HttpPut("{id}")]
        public async Task<ActionResult<DataResponse<UsuarioDto>>> PutUsuario(int id, UsuarioDto usuario)
        {

            DataResponse<UsuarioDto> response = new DataResponse<UsuarioDto>();
            if (id != usuario.UsuarioId)
            {
                response.Message = "Id de usuario incorrecto";
                return BadRequest(response);
            }

            _context.Entry(_mapper.Map<Usuario>(usuario)).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
                {
                    response.Message = "Usuario no encontrado";
                    return NotFound(response);
                }
                else
                {

                    return StatusCode(500, new DataResponse<UsuarioDto>(false, "Error interno del servidor"));
                }
            }

            response.IsSuccess = true;
            response.Message = "Usuario actualizado exitosamente";
            response.Result = usuario;

            return Ok(response);
        }

        // POST: api/Usuarios
        [HttpPost]
        public async Task<ActionResult<DataResponse<UsuarioDto>>> PostUsuario(UsuarioDto usuario)
        {
            try
            {
                var UsuarioData = _mapper.Map<Usuario>(usuario);
                _context.Usuarios.Add(UsuarioData);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetUsuarios", new { id = UsuarioData.UsuarioId }, new DataResponse<UsuarioDto>(true, "Cliente creado exitosamente", _mapper.Map<UsuarioDto>(UsuarioData)));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new DataResponse<UsuarioDto>(false, $"Error interno del servidor: {ex.Message}"));
            }
        }

        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<DataResponse<UsuarioDto>>> DeleteUsuario(int id)
        {
            try
            {
                var response = new DataResponse<UsuarioDto>();

                var usuario = await _context.Usuarios.FindAsync(id);
                if (usuario == null)
                {
                    response.Message = "Usuario no encontrado";

                    return NotFound(response);
                }

                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();

                response.IsSuccess = true;
                response.Message = "Usuario eliminado exitosamente";

                return Ok(response);


            }
            catch (Exception ex)
            {
                return StatusCode(500, new DataResponse<UsuarioDto>(false, $"Error interno del servidor: {ex.Message}"));
            }
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.UsuarioId == id);
        }
    }
}
