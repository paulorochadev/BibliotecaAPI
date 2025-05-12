using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.Models.Entities
{
    [Table("TipoCompra")]
    public class TipoCompra
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Required]
        [Column("Descricao", TypeName = "varchar(20)")]
        public string Descricao { get; set; }

        public ICollection<PrecoLivro> PrecosLivro { get; set; }
    }
}