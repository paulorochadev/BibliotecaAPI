using System.ComponentModel.DataAnnotations.Schema;

namespace BibliotecaAPI.Models.Entities
{
    [Table("Livro_Autor")]
    public class LivroAutor
    {
        [Column("Livro_Codi")]
        public int LivroId { get; set; }

        [Column("Autor_CodAu")]
        public int AutorId { get; set; }

        public Livro Livro { get; set; }
        public Autor Autor { get; set; }
    }
}