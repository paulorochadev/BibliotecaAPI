using AutoMapper;
using BibliotecaAPI.Models.DTOs;
using BibliotecaAPI.Models.Entities;

namespace BibliotecaAPI.Extensions
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapeamento para Livro
            CreateMap<Livro, LivroDTO>()
                .ReverseMap();

            // Mapeamento para Autor
            CreateMap<Autor, AutorDTO>()
                .ReverseMap();

            // Mapeamento para Assunto
            CreateMap<Assunto, AssuntoDTO>()
                .ReverseMap();

            // Mapeamento para PrecoLivro
            CreateMap<PrecoLivro, PrecoLivroDTO>()
                .ReverseMap();
        }
    }
}