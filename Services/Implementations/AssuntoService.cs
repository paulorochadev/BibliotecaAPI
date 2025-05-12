using AutoMapper;
using BibliotecaAPI.Models.DTOs;
using BibliotecaAPI.Models.Entities;
using BibliotecaAPI.Repositories.Interfaces;
using BibliotecaAPI.Services.Interfaces;

namespace BibliotecaAPI.Services.Implementations
{
    public class AssuntoService : IAssuntoService
    {
        private readonly IAssuntoRepository _assuntoRepository;
        private readonly IMapper _mapper;

        public AssuntoService(IAssuntoRepository assuntoRepository, IMapper mapper)
        {
            _assuntoRepository = assuntoRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AssuntoDTO>> GetAllAssuntosAsync()
        {
            var assuntos = await _assuntoRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<AssuntoDTO>>(assuntos);
        }

        public async Task<AssuntoDTO?> GetAssuntoByIdAsync(int id)
        {
            var assunto = await _assuntoRepository.GetByIdAsync(id);

            return _mapper.Map<AssuntoDTO>(assunto);
        }

        public async Task<AssuntoDTO> CreateAssuntoAsync(AssuntoDTO assuntoDto)
        {
            var assunto = _mapper.Map<Assunto>(assuntoDto);
            var createdAssunto = await _assuntoRepository.CreateAsync(assunto);

            return _mapper.Map<AssuntoDTO>(createdAssunto);
        }

        public async Task UpdateAssuntoAsync(int id, AssuntoDTO assuntoDto)
        {
            if (id != assuntoDto.Id)
            {
                throw new ArgumentException("ID do assunto não corresponde ao ID fornecido.");
            }

            var assunto = _mapper.Map<Assunto>(assuntoDto);

            await _assuntoRepository.UpdateAsync(assunto);
        }

        public async Task DeleteAssuntoAsync(int id)
        {
            await _assuntoRepository.DeleteAsync(id);
        }
    }
}