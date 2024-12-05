﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using digeset_server.Core.entities;
using Microsoft.EntityFrameworkCore;

namespace digeset_server.Infrastructure.Context;

public partial class digesetContext : DbContext
{
    public digesetContext(DbContextOptions<digesetContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Agente> Agentes { get; set; }

    public virtual DbSet<Concepto> Conceptos { get; set; }

    public virtual DbSet<Multa> Multa { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Agente>(entity =>
        {
            entity.HasKey(e => e.AgenteId).HasName("PK__Agente__EA09D85DF09E5DC3");

            entity.ToTable("Agente");

            entity.Property(e => e.Cedula)
                .IsRequired()
                .HasMaxLength(20);
            entity.Property(e => e.Clave)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(100);
        });

        modelBuilder.Entity<Concepto>(entity =>
        {
            entity.HasKey(e => e.ConceptoId).HasName("PK__Concepto__BB30F135A80A6E3B");

            entity.ToTable("Concepto");

            entity.Property(e => e.Descripcion)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(e => e.Monto).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<Multa>(entity =>
        {
            entity.HasKey(e => e.MultaId).HasName("PK__Multa__DA090DE0F9E588A4");

            entity.Property(e => e.Cedula)
                .IsRequired()
                .HasMaxLength(20);
            entity.Property(e => e.Descripcion).HasMaxLength(200);
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Latitud).HasColumnType("decimal(9, 6)");
            entity.Property(e => e.Longitud).HasColumnType("decimal(9, 6)");
            entity.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            entity.HasOne(d => d.Agente).WithMany(p => p.Multa)
                .HasForeignKey(d => d.AgenteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Multa__AgenteId__44FF419A");

            entity.HasOne(d => d.Concepto).WithMany(p => p.Multa)
                .HasForeignKey(d => d.ConceptoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Multa__ConceptoI__4316F928");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.UsuarioId).HasName("PK__Usuario__2B3DE7B84F7C7FC9");

            entity.ToTable("Usuario");

            entity.Property(e => e.Cedula)
                .IsRequired()
                .HasMaxLength(20);
            entity.Property(e => e.Clave)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}