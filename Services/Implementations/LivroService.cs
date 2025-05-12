using AutoMapper;
using BibliotecaAPI.Models.DTOs;
using BibliotecaAPI.Models.Entities;
using BibliotecaAPI.Repositories.Interfaces;
using BibliotecaAPI.Services.Interfaces;

namespace BibliotecaAPI.Services.Implementations
{
    public class LivroService : ILivroService
    {
        private readonly ILivroRepository _livroRepository;
        private readonly IMapper _mapper;

        public LivroService(ILivroRepository livroRepository, IMapper mapper)
        {
            _livroRepository = livroRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<LivroDTO>> GetAllLivrosAsync()
        {
            var livros = await _livroRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<LivroDTO>>(livros);
        }

        public async Task<LivroDTO?> GetLivroByIdAsync(int id)
        {
            var livro = await _livroRepository.GetByIdAsync(id);

            return _mapper.Map<LivroDTO>(livro);
        }

        public async Task<LivroDTO> CreateLivroAsync(LivroDTO livroDto)
        {
            var livro = _mapper.Map<Livro>(livroDto);

            // Tratar relacionamentos
            if (livroDto.Autores?.Any() == true)
            {
                livro.Autores = livroDto.Autores.Select(a => new LivroAutor
                {
                    LivroId = livro.Id,
                    AutorId = a.Id
                }).ToList();
            }

            var createdLivro = await _livroRepository.CreateAsync(livro);

            return _mapper.Map<LivroDTO>(createdLivro);
        }

        public async Task UpdateLivroAsync(int id, LivroDTO livroDto)
        {
            if (id != livroDto.Id)
            {
                throw new ArgumentException("ID do livro não corresponde ao ID fornecido.");
            }

            var livro = _mapper.Map<Livro>(livroDto);

            await _livroRepository.UpdateAsync(livro);
        }

        public async Task DeleteLivroAsync(int id)
        {
            await _livroRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<PrecoLivroDTO>> GetPrecosByLivroIdAsync(int livroId)
        {
            var livro = await _livroRepository.GetByIdAsync(livroId);

            if (livro == null)
            {
                throw new KeyNotFoundException($"Livro com ID {livroId} não encontrado.");
            }

            return _mapper.Map<IEnumerable<PrecoLivroDTO>>(livro.Precos);
        }

        public async Task<PrecoLivroDTO> AddPrecoToLivroAsync(int livroId, PrecoLivroDTO precoDto)
        {
            var livro = await _livroRepository.GetByIdAsync(livroId);

            if (livro == null)
            {
                throw new KeyNotFoundException($"Livro com ID {livroId} não encontrado.");
            }

            var preco = _mapper.Map<PrecoLivro>(precoDto);
            preco.LivroId = livroId;
            livro.Precos.Add(preco);

            await _livroRepository.UpdateAsync(livro);

            return _mapper.Map<PrecoLivroDTO>(preco);
        }
    }
}