namespace BibliotecaAPI.Models.DTOs
{
    public class RelatorioLivrosPorAutorDTO
    {
        public int AutorId { get; set; }
        public string AutorNome { get; set; }
        public int LivroId { get; set; }
        public string LivroTitulo { get; set; }
        public string Editora { get; set; }
        public int Edicao { get; set; }
        public string AnoPublicacao { get; set; }
        public string Assuntos { get; set; }
        public string Precos { get; set; }
        public int TotalLivros { get; set; }
    }
}