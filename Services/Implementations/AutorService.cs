using AutoMapper;
using BibliotecaAPI.Models.DTOs;
using BibliotecaAPI.Models.Entities;
using BibliotecaAPI.Repositories.Interfaces;
using BibliotecaAPI.Services.Interfaces;

namespace BibliotecaAPI.Services.Implementations
{
    public class AutorService : IAutorService
    {
        private readonly IAutorRepository _autorRepository;
        private readonly IMapper _mapper;

        public AutorService(IAutorRepository autorRepository, IMapper mapper)
        {
            _autorRepository = autorRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AutorDTO>> GetAllAutoresAsync()
        {
            var autores = await _autorRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<AutorDTO>>(autores);
        }

        public async Task<AutorDTO?> GetAutorByIdAsync(int id)
        {
            var autor = await _autorRepository.GetByIdAsync(id);

            return _mapper.Map<AutorDTO>(autor);
        }

        public async Task<AutorDTO> CreateAutorAsync(AutorDTO autorDto)
        {
            var autor = _mapper.Map<Autor>(autorDto);
            var createdAutor = await _autorRepository.CreateAsync(autor);

            return _mapper.Map<AutorDTO>(createdAutor);
        }

        public async Task UpdateAutorAsync(int id, AutorDTO autorDto)
        {
            if (id != autorDto.Id)
            {
                throw new ArgumentException("ID do autor não corresponde ao ID fornecido.");
            }

            var autor = _mapper.Map<Autor>(autorDto);

            await _autorRepository.UpdateAsync(autor);
        }

        public async Task DeleteAutorAsync(int id)
        {
            await _autorRepository.DeleteAsync(id);
        }
    }
}