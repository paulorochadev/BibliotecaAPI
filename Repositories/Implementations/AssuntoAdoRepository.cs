using BibliotecaAPI.Models.Entities;
using BibliotecaAPI.Repositories.Interfaces;
using Dapper;
using System.Data;

namespace BibliotecaAPI.Repositories.Implementations
{
    public class AssuntoAdoRepository : IAssuntoRepository
    {
        private readonly IDbConnection _connection;

        public AssuntoAdoRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<Assunto>> GetAllAsync()
        {
            return await _connection.QueryAsync<Assunto>("SELECT codAs as Id, Descricao FROM Assunto");
        }

        public async Task<Assunto?> GetByIdAsync(int id)
        {
            return await _connection.QueryFirstOrDefaultAsync<Assunto>(
                "SELECT codAs as Id, Descricao FROM Assunto WHERE codAs = @Id",
                new { Id = id });
        }

        public async Task<Assunto> CreateAsync(Assunto assunto)
        {
            var query = @"
                INSERT INTO Assunto (Descricao)
                OUTPUT INSERTED.codAs AS Id, INSERTED.Descricao
                VALUES (@Descricao)";

            return await _connection.QuerySingleAsync<Assunto>(query, assunto);
        }

        public async Task UpdateAsync(Assunto assunto)
        {
            await _connection.ExecuteAsync(
                "UPDATE Assunto SET Descricao = @Descricao WHERE codAs = @Id",
                new { assunto.Descricao, Id = assunto.Id });
        }

        public async Task DeleteAsync(int id)
        {
            await _connection.ExecuteAsync(
                "DELETE FROM Assunto WHERE codAs = @Id",
                new { Id = id });
        }
    }
}