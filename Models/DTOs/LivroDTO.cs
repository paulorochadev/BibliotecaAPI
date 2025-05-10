namespace BibliotecaAPI.Models.DTOs
{
    public class LivroDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Editora { get; set; }
        public int Edicao { get; set; }
        public string AnoPublicacao { get; set; }
        public List<AutorDTO> Autores { get; set; }
        public List<AssuntoDTO> Assuntos { get; set; }
    }
}