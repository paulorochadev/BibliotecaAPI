using BibliotecaAPI.Models.Entities;

namespace BibliotecaAPI.Repositories.Interfaces
{
    public interface IAssuntoRepository
    {
        Task<IEnumerable<Assunto>> GetAllAsync();
        Task<Assunto?> GetByIdAsync(int id);
        Task<Assunto> CreateAsync(Assunto assunto);
        Task UpdateAsync(Assunto assunto);
        Task DeleteAsync(int id);
    }
}