using BibliotecaAPI.Models.Entities;

namespace BibliotecaAPI.Repositories.Interfaces
{
    public interface ILivroRepository
    {
        Task<IEnumerable<Livro>> GetAllAsync();
        Task<Livro?> GetByIdAsync(int id);
        Task<Livro> CreateAsync(Livro livro);
        Task UpdateAsync(Livro livro);
        Task DeleteAsync(int id);
    }
}