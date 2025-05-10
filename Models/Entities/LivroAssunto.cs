using System.ComponentModel.DataAnnotations.Schema;

namespace BibliotecaAPI.Models.Entities
{
    [Table("Livro_Assunto")]
    public class LivroAssunto
    {
        [Column("Livro_Codi")]
        public int LivroId { get; set; }

        [Column("Assunto_codAs")]
        public int AssuntoId { get; set; }

        public Livro Livro { get; set; }
        public Assunto Assunto { get; set; }
    }
}