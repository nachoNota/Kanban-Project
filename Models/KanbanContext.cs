using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace tl2_proyecto_2024_nachoNota.Models;

public partial class KanbanContext : DbContext
{
    public KanbanContext()
    {
    }

    public KanbanContext(DbContextOptions<KanbanContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Passwordreset> Passwordresets { get; set; }

    public virtual DbSet<Tablero> Tableros { get; set; }

    public virtual DbSet<Tarea> Tareas { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Passwordreset>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("passwordreset");

            entity.HasIndex(e => e.IdUsuario, "FK_id_usuario_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Expiration)
                .HasColumnType("datetime")
                .HasColumnName("expiration");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Token)
                .HasMaxLength(255)
                .HasColumnName("token");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Passwordresets)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK_id_usuario");
        });

        modelBuilder.Entity<Tablero>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tablero");

            entity.HasIndex(e => e.IdUsuario, "fk_usuario_1_idx");

            entity.Property(e => e.Id).HasColumnName("id_tablero");
            entity.Property(e => e.Color)
                .HasMaxLength(45)
                .HasDefaultValueSql("'#ffffff'")
                .HasColumnName("color");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .HasColumnName("descripcion");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Titulo)
                .HasMaxLength(20)
                .HasColumnName("titulo");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Tableros)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_usuario_1");
        });

        modelBuilder.Entity<Tarea>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tarea");

            entity.HasIndex(e => e.IdTablero, "fk_tarea_tablero_idx");

            entity.Property(e => e.Id).HasColumnName("id_tarea");
            entity.Property(e => e.Color)
                .HasMaxLength(30)
                .HasDefaultValueSql("'#ffffff'")
                .HasColumnName("color");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(200)
                .HasColumnName("descripcion");
            entity.Property(e => ((int)e.Estado)).HasColumnName("estado");
            entity.Property(e => e.FechaModificacion)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("fecha_modificacion");
            entity.Property(e => e.IdTablero).HasColumnName("id_tablero");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Titulo)
                .HasMaxLength(45)
                .HasColumnName("titulo");

            entity.HasOne(d => d.IdTableroNavigation).WithMany(p => p.Tareas)
                .HasForeignKey(d => d.IdTablero)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_tarea_tablero");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("usuario");

            entity.HasIndex(e => e.Email, "email_UNIQUE").IsUnique();

            entity.HasIndex(e => e.NombreUsuario, "nombre_usuario_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id_usuario");
            entity.Property(e => e.Password)
                .HasMaxLength(90)
                .HasColumnName("contrasenia");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .HasColumnName("email");
            entity.Property(e => e.NombreUsuario)
                .HasMaxLength(20)
                .HasColumnName("nombre_usuario");
            entity.Property(e => ((int)e.Rol)).HasColumnName("rol");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
