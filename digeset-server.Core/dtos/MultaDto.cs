﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace digeset_server.Core.dtos;

public partial class MultaDto
{
    public int MultaId { get; set; }

    public string Cedula { get; set; }

    public string Nombre { get; set; }

    public int ConceptoId { get; set; }

    public string Descripcion { get; set; }

    public decimal Latitud { get; set; }

    public decimal Longitud { get; set; }

    public byte[] Foto { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public int EstadoId { get; set; }

}