﻿using AutoMapper;
using digeset_server.Core.contracts;
using digeset_server.Core.dtos;
using digeset_server.Core.entities;
using digeset_server.Infrastructure.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Solurecwebapi.Reponse;

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
        public async Task<ActionResult<DataResponse<object>>> LoginAgente(string tipo, [FromBody] LoginRequest req)
        {
            try
            {
                if (req == null || string.IsNullOrWhiteSpace(req.cedula) || string.IsNullOrWhiteSpace(req.clave))
                {
                    return BadRequest(new DataResponse<object>(false, "La cédula y la clave son obligatorias"));
                }

                var encontrado = new object();

                if (tipo == "Agente")
                {
                    encontrado = await _context.Agentes
                        .FirstOrDefaultAsync(a => a.Cedula == req.cedula && a.Clave == req.clave);
                }
                else
                {
                    encontrado = await _context.Usuarios
                        .FirstOrDefaultAsync(u => u.Cedula == req.cedula && u.Clave == req.clave);
                }

                if (encontrado == null)
                {
                    return NotFound(new DataResponse<object>(false, "Usuario o clave incorrecta"));
                }

                return Ok(new DataResponse<object>(true, "Inicio de sesión exitoso", encontrado));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new DataResponse<object>(false, $"Error interno del servidor: {ex.Message}"));
            }
        }


    }
}
