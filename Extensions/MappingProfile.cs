using AutoMapper;
using BibliotecaAPI.Models.DTOs;
using BibliotecaAPI.Models.Entities;
using System.Linq;

namespace BibliotecaAPI.Extensions
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapeamento para Livro
            CreateMap<Livro, LivroDTO>()
                .ForMember(dest => dest.Autores, opt => opt.MapFrom(src => src.Autores.Select(la => la.Autor)))
                .ForMember(dest => dest.Assuntos, opt => opt.MapFrom(src => src.Assuntos.Select(la => la.Assunto)))
                .ForMember(dest => dest.Precos, opt => opt.MapFrom(src => src.Precos))
                .ReverseMap()
                .ForMember(dest => dest.Autores, opt => opt.Ignore())
                .ForMember(dest => dest.Assuntos, opt => opt.Ignore())
                .ForMember(dest => dest.Precos, opt => opt.Ignore());

            // Mapeamento para Autor
            CreateMap<Autor, AutorDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Nome))
                .ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Nome));

            // Mapeamento para Assunto
            CreateMap<Assunto, AssuntoDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.Descricao))
                .ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.Descricao));

            // Mapeamento para PrecoLivro
            CreateMap<PrecoLivro, PrecoLivroDTO>()
                .ForMember(dest => dest.TipoCompraDescricao, opt => opt.MapFrom(src => src.TipoCompra != null ? src.TipoCompra.Descricao : string.Empty))
                .ReverseMap()
                .ForMember(dest => dest.TipoCompra, opt => opt.Ignore());

            // Mapeamento para TipoCompra
            CreateMap<TipoCompra, TipoCompraDTO>()
                .ReverseMap();

            // Mapeamento para a view de relatório
            CreateMap<RelatorioLivrosPorAutorDTO, RelatorioLivrosPorAutorDTO>();
        }
    }
}