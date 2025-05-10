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
            return await _context.Livros
                .Include(l => l.Autores)
                .ThenInclude(la => la.Autor)
                .Include(l => l.Assuntos)
                .ThenInclude(la => la.Assunto)
                .ToListAsync();
        }

        public async Task<Livro?> GetByIdAsync(int id)
        {
            return await _context.Livros
                .Include(l => l.Autores)
                .ThenInclude(la => la.Autor)
                .Include(l => l.Assuntos)
                .ThenInclude(la => la.Assunto)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<Livro> CreateAsync(Livro livro)
        {
            _context.Livros.Add(livro);

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
            var livro = await _context.Livros.FindAsync(id);

            if (livro != null)
            {
                _context.Livros.Remove(livro);

                await _context.SaveChangesAsync();
            }
        }
    }
}