using BibliotecaAPI.Models.DTOs;
using BibliotecaAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace BibliotecaAPI.Repositories
{
    public class BibliotecaContext : DbContext
    {
        public BibliotecaContext(DbContextOptions<BibliotecaContext> options) : base(options)
        {
        }

        // DbSets para cada entidade
        public DbSet<Livro> Livro { get; set; }
        public DbSet<Autor> Autor { get; set; }
        public DbSet<Assunto> Assunto { get; set; }
        public DbSet<LivroAutor> LivroAutor { get; set; }
        public DbSet<LivroAssunto> LivroAssunto { get; set; }
        public DbSet<PrecoLivro> LivroPreco { get; set; }
        public DbSet<TipoCompra> TipoCompra { get; set; }

        // DbSet para view de relatório
        public DbSet<RelatorioLivrosPorAutorDTO> RelatorioLivrosPorAutor { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("name=ConnectionStrings:BibliotecaConnection");
                optionsBuilder.EnableSensitiveDataLogging();
                optionsBuilder.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Configuração de Tabelas e Colunas
            modelBuilder.Entity<Livro>(entity =>
            {
                entity.ToTable("Livro");
                entity.Property(e => e.Id).HasColumnName("Codi");
                entity.Property(e => e.Titulo).HasMaxLength(40).IsRequired();
                entity.Property(e => e.Editora).HasMaxLength(40).IsRequired();
                entity.Property(e => e.AnoPublicacao).HasMaxLength(4).IsRequired();
                entity.HasIndex(e => e.Titulo).HasDatabaseName("IX_Livro_Titulo");
            });

            modelBuilder.Entity<Autor>(entity =>
            {
                entity.ToTable("Autor");
                entity.Property(e => e.Id).HasColumnName("CodAu");
                entity.Property(e => e.Nome).HasMaxLength(40).IsRequired();
                entity.HasIndex(e => e.Nome).HasDatabaseName("IX_Autor_Nome");
            });

            modelBuilder.Entity<Assunto>(entity =>
            {
                entity.ToTable("Assunto");
                entity.Property(e => e.Id).HasColumnName("codAs");
                entity.Property(e => e.Descricao).HasMaxLength(20).IsRequired();
                entity.HasIndex(e => e.Descricao).HasDatabaseName("IX_Assunto_Descricao");
            });

            modelBuilder.Entity<LivroAutor>(entity =>
            {
                entity.ToTable("Livro_Autor");
                entity.Property(e => e.LivroId).HasColumnName("Livro_Codi");
                entity.Property(e => e.AutorId).HasColumnName("Autor_CodAu");
            });

            modelBuilder.Entity<LivroAssunto>(entity =>
            {
                entity.ToTable("Livro_Assunto");
                entity.Property(e => e.LivroId).HasColumnName("Livro_Codi");
                entity.Property(e => e.AssuntoId).HasColumnName("Assunto_codAs");
            });

            modelBuilder.Entity<PrecoLivro>(entity =>
            {
                entity.ToTable("LivroPreco");
                entity.Property(e => e.Id).HasColumnName("Id");
                entity.Property(e => e.LivroId).HasColumnName("Livro_Codi");
                entity.Property(e => e.TipoCompraId).HasColumnName("TipoCompra_Id");
                entity.Property(e => e.Valor).HasColumnName("Preco").HasColumnType("decimal(10,2)").IsRequired();
                entity.HasIndex(e => e.LivroId).HasDatabaseName("IX_LivroPreco_LivroId");
            });

            modelBuilder.Entity<TipoCompra>(entity =>
            {
                entity.ToTable("TipoCompra");
                entity.Property(e => e.Id).HasColumnName("Id");
                entity.Property(e => e.Descricao).HasMaxLength(20).IsRequired();
                entity.HasIndex(e => e.Descricao).IsUnique().HasDatabaseName("UQ_TipoCompra_Descricao");
            });
            #endregion

            #region Configuração de Chaves Primárias
            modelBuilder.Entity<LivroAutor>()
                .HasKey(la => new { la.LivroId, la.AutorId })
                .HasName("PK_Livro_Autor");

            modelBuilder.Entity<LivroAssunto>()
                .HasKey(la => new { la.LivroId, la.AssuntoId })
                .HasName("PK_Livro_Assunto");
            #endregion

            #region Configuração de Relacionamentos
            modelBuilder.Entity<LivroAutor>()
                .HasOne(la => la.Livro)
                .WithMany(l => l.Autores)
                .HasForeignKey(la => la.LivroId)
                .HasConstraintName("FK_Livro_Autor_Livro")
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LivroAutor>()
                .HasOne(la => la.Autor)
                .WithMany(a => a.Livros)
                .HasForeignKey(la => la.AutorId)
                .HasConstraintName("FK_Livro_Autor_Autor")
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LivroAssunto>()
                .HasOne(la => la.Livro)
                .WithMany(l => l.Assuntos)
                .HasForeignKey(la => la.LivroId)
                .HasConstraintName("FK_Livro_Assunto_Livro")
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LivroAssunto>()
                .HasOne(la => la.Assunto)
                .WithMany(a => a.Livros)
                .HasForeignKey(la => la.AssuntoId)
                .HasConstraintName("FK_Livro_Assunto_Assunto")
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PrecoLivro>()
                .HasOne(pl => pl.Livro)
                .WithMany(l => l.Precos)
                .HasForeignKey(pl => pl.LivroId)
                .HasConstraintName("FK_LivroPreco_Livro")
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PrecoLivro>()
                .HasOne(pl => pl.TipoCompra)
                .WithMany()
                .HasForeignKey(pl => pl.TipoCompraId)
                .HasConstraintName("FK_LivroPreco_TipoCompra")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PrecoLivro>()
                .HasIndex(pl => new { pl.LivroId, pl.TipoCompraId })
                .IsUnique()
                .HasDatabaseName("UQ_Livro_TipoCompra");
            #endregion

            #region Configuração de Views e DTOs
            modelBuilder.Entity<RelatorioLivrosPorAutorDTO>()
                .HasNoKey()
                .ToView("vw_RelatorioLivrosPorAutor");

            modelBuilder.Entity<LivroDTO>().HasNoKey();
            modelBuilder.Entity<AutorDTO>().HasNoKey();
            modelBuilder.Entity<AssuntoDTO>().HasNoKey();
            modelBuilder.Entity<PrecoLivroDTO>().HasNoKey();
            #endregion
        }
    }
}