using BibliotecaAPI.Models.DTOs;

namespace BibliotecaAPI.Services.Interfaces
{
    public interface ILivroService
    {
        Task<IEnumerable<LivroDTO>> GetAllLivrosAsync();
        Task<LivroDTO?> GetLivroByIdAsync(int id);
        Task<LivroDTO> CreateLivroAsync(LivroDTO livroDto);
        Task UpdateLivroAsync(int id, LivroDTO livroDto);
        Task DeleteLivroAsync(int id);
        Task<IEnumerable<PrecoLivroDTO>> GetPrecosByLivroIdAsync(int livroId);
        Task<PrecoLivroDTO> AddPrecoToLivroAsync(int livroId, PrecoLivroDTO precoDto);
    }
}