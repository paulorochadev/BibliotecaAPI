using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.Models.Entities
{
    [Table("Assunto")]
    public class Assunto
    {
        [Key]
        [Column("codAs")]
        public int Id { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "A descrição não pode exceder 20 caracteres.")]
        [Column("Descricao")]
        public string Descricao { get; set; }

        // Relacionamento N-N com Livro
        public ICollection<LivroAssunto> Livros { get; set; }
    }
}