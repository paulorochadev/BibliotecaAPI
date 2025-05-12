using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.Models.DTOs
{
    public class TipoCompraDTO
    {
        public int Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
    }
}