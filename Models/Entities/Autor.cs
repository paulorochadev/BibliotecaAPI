using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.Models.Entities
{
    [Table("Autor")]
    public class Autor
    {
        [Key]
        [Column("CodAu")]
        public int Id { get; set; }

        [Required]
        [StringLength(40, ErrorMessage = "O nome não pode exceder 40 caracteres.")]
        [Column("Nome")]
        public string Nome { get; set; }

        // Relacionamento N-N com Livro
        public ICollection<LivroAutor> Livros { get; set; }
    }
}