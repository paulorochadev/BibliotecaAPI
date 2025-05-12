using BibliotecaAPI.Models.Entities;
using BibliotecaAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaAPI.Repositories.Implementations
{
    public class AutorRepository : IAutorRepository
    {
        private readonly BibliotecaContext _context;

        public AutorRepository(BibliotecaContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Autor>> GetAllAsync()
        {
            return await _context.Autor.ToListAsync();
        }

        public async Task<Autor?> GetByIdAsync(int id)
        {
            return await _context.Autor.FindAsync(id);
        }

        public async Task<Autor> CreateAsync(Autor autor)
        {
            _context.Autor.Add(autor);

            await _context.SaveChangesAsync();

            return autor;
        }

        public async Task UpdateAsync(Autor autor)
        {
            _context.Entry(autor).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var autor = await _context.Autor.FindAsync(id);

            if (autor != null)
            {
                _context.Autor.Remove(autor);

                await _context.SaveChangesAsync();
            }
        }
    }
}