using BibliotecaAPI.Models.DTOs;

namespace BibliotecaAPI.Services.Interfaces
{
    public interface IAssuntoService
    {
        Task<IEnumerable<AssuntoDTO>> GetAllAssuntosAsync();
        Task<AssuntoDTO?> GetAssuntoByIdAsync(int id);
        Task<AssuntoDTO> CreateAssuntoAsync(AssuntoDTO assuntoDto);
        Task UpdateAssuntoAsync(int id, AssuntoDTO assuntoDto);
        Task DeleteAssuntoAsync(int id);
    }
}