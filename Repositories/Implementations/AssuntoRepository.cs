using BibliotecaAPI.Models.Entities;
using BibliotecaAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaAPI.Repositories.Implementations
{
    public class AssuntoRepository : IAssuntoRepository
    {
        private readonly BibliotecaContext _context;

        public AssuntoRepository(BibliotecaContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Assunto>> GetAllAsync()
        {
            return await _context.Assuntos.ToListAsync();
        }

        public async Task<Assunto?> GetByIdAsync(int id)
        {
            return await _context.Assuntos.FindAsync(id);
        }

        public async Task<Assunto> CreateAsync(Assunto assunto)
        {
            _context.Assuntos.Add(assunto);

            await _context.SaveChangesAsync();

            return assunto;
        }

        public async Task UpdateAsync(Assunto assunto)
        {
            _context.Entry(assunto).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var assunto = await _context.Assuntos.FindAsync(id);

            if (assunto != null)
            {
                _context.Assuntos.Remove(assunto);

                await _context.SaveChangesAsync();
            }
        }
    }
}