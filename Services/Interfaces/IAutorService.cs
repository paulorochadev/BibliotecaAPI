using BibliotecaAPI.Models.DTOs;

namespace BibliotecaAPI.Services.Interfaces
{
    public interface IAutorService
    {
        Task<IEnumerable<AutorDTO>> GetAllAutoresAsync();
        Task<AutorDTO?> GetAutorByIdAsync(int id);
        Task<AutorDTO> CreateAutorAsync(AutorDTO autorDto);
        Task UpdateAutorAsync(int id, AutorDTO autorDto);
        Task DeleteAutorAsync(int id);
    }
}