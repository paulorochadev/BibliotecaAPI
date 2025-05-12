using BibliotecaAPI.Models.Entities;
using BibliotecaAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaAPI.Repositories.Implementations
{
    public class LivroRepository : ILivroRepository
    {
        private readonly BibliotecaContext _context;

        public LivroRepository(BibliotecaContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Livro>> GetAllAsync()
        {
            return await _context.Livro
                .Include(l => l.Autores)
                    .ThenInclude(la => la.Autor)
                .Include(l => l.Assuntos)
                    .ThenInclude(la => la.Assunto)
                .Include(l => l.Precos)
                    .ThenInclude(p => p.TipoCompra)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Livro?> GetByIdAsync(int id)
        {
            return await _context.Livro
                .Include(l => l.Autores)
                    .ThenInclude(la => la.Autor)
                .Include(l => l.Assuntos)
                    .ThenInclude(la => la.Assunto)
                .Include(l => l.Precos)
                    .ThenInclude(p => p.TipoCompra)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<Livro> CreateAsync(Livro livro)
        {
            _context.Livro.Add(livro);
            await _context.SaveChangesAsync();
            return livro;
        }

        public async Task UpdateAsync(Livro livro)
        {
            _context.Entry(livro).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var livro = await _context.Livro.FindAsync(id);
            if (livro != null)
            {
                _context.Livro.Remove(livro);
                await _context.SaveChangesAsync();
            }
        }
    }
}