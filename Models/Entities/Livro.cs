using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BibliotecaAPI.Models.Entities
{
    public class Livro
    {
        [Key]
        [Column("Codi")]
        public int Id { get; set; }

        [Required]
        [StringLength(40, ErrorMessage = "O título não pode exceder 40 caracteres.")]
        [Column("Titulo")]
        public string Titulo { get; set; }

        [Required]
        [StringLength(40, ErrorMessage = "A editora não pode exceder 40 caracteres.")]
        [Column("Editora")]
        public string Editora { get; set; }

        [Required]
        [Column("Edicao")]
        public int Edicao { get; set; }

        [Required]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "O ano deve ter exatamente 4 caracteres.")]
        [Column("AnoPublicacao")]
        public string AnoPublicacao { get; set; }

        // Relacionamento N-N com Autor
        public ICollection<LivroAutor> Autores { get; set; }
        
        // Relacionamento N-N com Assunto
        public ICollection<LivroAssunto> Assuntos { get; set; }

        // Relacionamento 1-N com PrecoLivro
        public ICollection<PrecoLivro> Precos { get; set; }
    }
}