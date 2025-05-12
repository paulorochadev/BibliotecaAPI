using BibliotecaAPI.Models.Entities;
using BibliotecaAPI.Repositories.Interfaces;
using Dapper;
using System.Data;

namespace BibliotecaAPI.Repositories.Implementations
{
    public class AutorAdoRepository : IAutorRepository
    {
        private readonly IDbConnection _connection;

        public AutorAdoRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<Autor>> GetAllAsync()
        {
            return await _connection.QueryAsync<Autor>("SELECT CodAu as Id, Nome FROM Autor");
        }

        public async Task<Autor?> GetByIdAsync(int id)
        {
            return await _connection.QueryFirstOrDefaultAsync<Autor>(
                "SELECT CodAu as Id, Nome FROM Autor WHERE CodAu = @Id",
                new { Id = id });
        }

        public async Task<Autor> CreateAsync(Autor autor)
        {
            var query = @"
                INSERT INTO Autor (Nome)
                OUTPUT INSERTED.CodAu AS Id, INSERTED.Nome
                VALUES (@Nome)";

            return await _connection.QuerySingleAsync<Autor>(query, autor);
        }

        public async Task UpdateAsync(Autor autor)
        {
            await _connection.ExecuteAsync(
                "UPDATE Autor SET Nome = @Nome WHERE CodAu = @Id",
                new { autor.Nome, Id = autor.Id });
        }

        public async Task DeleteAsync(int id)
        {
            await _connection.ExecuteAsync(
                "DELETE FROM Autor WHERE CodAu = @Id",
                new { Id = id });
        }
    }
}