using BibliotecaAPI.Models.Entities;

namespace BibliotecaAPI.Repositories.Interfaces
{
    public interface IAutorRepository
    {
        Task<IEnumerable<Autor>> GetAllAsync();
        Task<Autor?> GetByIdAsync(int id);
        Task<Autor> CreateAsync(Autor autor);
        Task UpdateAsync(Autor autor);
        Task DeleteAsync(int id);
    }
}