using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.Models.Entities
{
    [Table("LivroPreco")]
    public class PrecoLivro
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("Livro_Codi")]
        public int LivroId { get; set; }

        [Column("TipoCompra_Id")]
        public int TipoCompraId { get; set; }

        [Column("Preco")]
        public decimal Valor { get; set; }

        [ForeignKey("LivroId")]
        public Livro Livro { get; set; }

        [ForeignKey("TipoCompraId")]
        public TipoCompra TipoCompra { get; set; }
    }
}