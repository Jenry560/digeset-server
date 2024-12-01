using AutoMapper;
using digeset_server.Core.contracts;
using digeset_server.Core.dtos;
using digeset_server.Core.entities;
using digeset_server.Infrastructure.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace digeset_server.Api.Controllers
{
    public class AuthController : Controller
    {
        private readonly digesetContext _context;
        private readonly IMapper _mapper;

        public AuthController(digesetContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpPut("Login/{tipo}")]
        public async Task<ActionResult> LoginAgente(string tipo, LoginRequest req)
        {
            if (tipo == "Agente")
            {
                var encontrado = await _context.Agentes.FirstOrDefaultAsync(a => a.Cedula == req.cedula && a.Clave == req.clave);
                if (encontrado == null)
                {
                    return NotFound(new { success = false, message = "Usuario o Clave incorrecta" });
                }
            }
            else
            {
                var encontrado = await _context.Usuarios.FirstOrDefaultAsync(a => a.Cedula == req.cedula && a.Clave == req.clave);
                if (encontrado == null)
                {
                    return NotFound(new { success = false, message = "Usuario o Clave incorrecta" });
                }
            }

            return Ok();
        }

    }
}
