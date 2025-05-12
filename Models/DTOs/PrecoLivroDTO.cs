namespace BibliotecaAPI.Models.DTOs
{
    public class PrecoLivroDTO
    {
        public int Id { get; set; }
        public int LivroId { get; set; }
        public decimal Valor { get; set; }
        public string TipoCompra { get; set; }
    }
}