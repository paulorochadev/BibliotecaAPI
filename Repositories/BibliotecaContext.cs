using BibliotecaAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaAPI.Repositories
{
    public class BibliotecaContext : DbContext
    {
        public BibliotecaContext(DbContextOptions<BibliotecaContext> options) : base(options)
        {
        }

        // DbSets para cada entidade
        public DbSet<Livro> Livros { get; set; }
        public DbSet<Autor> Autores { get; set; }
        public DbSet<Assunto> Assuntos { get; set; }
        public DbSet<LivroAutor> LivroAutores { get; set; }
        public DbSet<LivroAssunto> LivroAssuntos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuração das chaves tabelas de relacionamento
            modelBuilder.Entity<LivroAutor>()
                .HasKey(la => new { la.LivroId, la.AutorId });

            modelBuilder.Entity<LivroAssunto>()
                .HasKey(la => new { la.LivroId, la.AssuntoId });

            // Configuração dos relacionamentos
            modelBuilder.Entity<LivroAutor>()
                .HasOne(la => la.Livro)
                .WithMany(l => l.Autores)
                .HasForeignKey(la => la.LivroId);

            modelBuilder.Entity<LivroAutor>()
                .HasOne(la => la.Autor)
                .WithMany(a => a.Livros)
                .HasForeignKey(la => la.AutorId);

            modelBuilder.Entity<LivroAssunto>()
                .HasOne(la => la.Livro)
                .WithMany(l => l.Assuntos)
                .HasForeignKey(la => la.LivroId);

            modelBuilder.Entity<LivroAssunto>()
                .HasOne(la => la.Assunto)
                .WithMany(a => a.Livros)
                .HasForeignKey(la => la.AssuntoId);

            // Configuração dos tamanhos dos campos
            modelBuilder.Entity<Livro>()
                .Property(l => l.Titulo)
                .HasMaxLength(40);

            modelBuilder.Entity<Livro>()
                .Property(l => l.Editora)
                .HasMaxLength(40);

            modelBuilder.Entity<Livro>()
                .Property(l => l.AnoPublicacao)
                .HasMaxLength(4);

            modelBuilder.Entity<Autor>()
                .Property(a => a.Nome)
                .HasMaxLength(40);

            modelBuilder.Entity<Assunto>()
                .Property(a => a.Descricao)
                .HasMaxLength(20);
        }
    }
}