using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.Models.Entities
{
    public class PrecoLivro
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column("Livro_Codi")]
        public int LivroId { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Valor { get; set; }

        [Required]
        [StringLength(20)]
        public string TipoCompra { get; set; } // "balcao", "self-service", "internet", "evento"

        public Livro Livro { get; set; }
    }
}