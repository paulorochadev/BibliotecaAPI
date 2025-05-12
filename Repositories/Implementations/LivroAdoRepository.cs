using BibliotecaAPI.Models.Entities;
using BibliotecaAPI.Repositories.Interfaces;
using Dapper;
using System.Data;
using System.Transactions;

namespace BibliotecaAPI.Repositories.Implementations
{
    public class LivroAdoRepository : ILivroRepository
    {
        private readonly IDbConnection _connection;

        public LivroAdoRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<Livro>> GetAllAsync()
        {
            var livroDictionary = new Dictionary<int, Livro>();

            var query = @"
                SELECT l.*, 
                       a.CodAu as Id, a.Nome,
                       ass.codAs as Id, ass.Descricao,
                       lp.Id, lp.Livro_Codi as LivroId, lp.TipoCompra_Id as TipoCompraId, lp.Preco as Valor,
                       tc.Id, tc.Descricao
                FROM Livro l
                LEFT JOIN Livro_Autor la ON l.Codi = la.Livro_Codi
                LEFT JOIN Autor a ON la.Autor_CodAu = a.CodAu
                LEFT JOIN Livro_Assunto las ON l.Codi = las.Livro_Codi
                LEFT JOIN Assunto ass ON las.Assunto_codAs = ass.codAs
                LEFT JOIN LivroPreco lp ON l.Codi = lp.Livro_Codi
                LEFT JOIN TipoCompra tc ON lp.TipoCompra_Id = tc.Id";

            var livros = await _connection.QueryAsync<Livro, Autor, Assunto, PrecoLivro, TipoCompra, Livro>(
                query,
                (livro, autor, assunto, precoLivro, tipoCompra) =>
                {
                    if (!livroDictionary.TryGetValue(livro.Id, out var livroEntry))
                    {
                        livroEntry = livro;
                        livroEntry.Autores = new List<LivroAutor>();
                        livroEntry.Assuntos = new List<LivroAssunto>();
                        livroEntry.Precos = new List<PrecoLivro>();
                        livroDictionary.Add(livroEntry.Id, livroEntry);
                    }

                    if (autor != null && !livroEntry.Autores.Any(a => a.AutorId == autor.Id))
                    {
                        livroEntry.Autores.Add(new LivroAutor
                        {
                            LivroId = livro.Id,
                            AutorId = autor.Id,
                            Autor = autor
                        });
                    }

                    if (assunto != null && !livroEntry.Assuntos.Any(a => a.AssuntoId == assunto.Id))
                    {
                        livroEntry.Assuntos.Add(new LivroAssunto
                        {
                            LivroId = livro.Id,
                            AssuntoId = assunto.Id,
                            Assunto = assunto
                        });
                    }

                    if (precoLivro != null && !livroEntry.Precos.Any(p => p.Id == precoLivro.Id))
                    {
                        precoLivro.TipoCompra = tipoCompra;
                        livroEntry.Precos.Add(precoLivro);
                    }

                    return livroEntry;
                },
                splitOn: "Id,Id,Id,Id");

            return livroDictionary.Values;
        }

        public async Task<Livro?> GetByIdAsync(int id)
        {
            var livroDictionary = new Dictionary<int, Livro>();

            var query = @"
                SELECT l.*, 
                       a.CodAu as Id, a.Nome,
                       ass.codAs as Id, ass.Descricao,
                       lp.Id, lp.Livro_Codi as LivroId, lp.TipoCompra_Id as TipoCompraId, lp.Preco as Valor,
                       tc.Id, tc.Descricao
                FROM Livro l
                LEFT JOIN Livro_Autor la ON l.Codi = la.Livro_Codi
                LEFT JOIN Autor a ON la.Autor_CodAu = a.CodAu
                LEFT JOIN Livro_Assunto las ON l.Codi = las.Livro_Codi
                LEFT JOIN Assunto ass ON las.Assunto_codAs = ass.codAs
                LEFT JOIN LivroPreco lp ON l.Codi = lp.Livro_Codi
                LEFT JOIN TipoCompra tc ON lp.TipoCompra_Id = tc.Id
                WHERE l.Codi = @Id";

            await _connection.QueryAsync<Livro, Autor, Assunto, PrecoLivro, TipoCompra, Livro>(
                query,
                (livro, autor, assunto, precoLivro, tipoCompra) =>
                {
                    if (!livroDictionary.TryGetValue(livro.Id, out var livroEntry))
                    {
                        livroEntry = livro;
                        livroEntry.Autores = new List<LivroAutor>();
                        livroEntry.Assuntos = new List<LivroAssunto>();
                        livroEntry.Precos = new List<PrecoLivro>();
                        livroDictionary.Add(livroEntry.Id, livroEntry);
                    }

                    return livroEntry;
                },
                new { Id = id },
                splitOn: "Id,Id,Id,Id");

            return livroDictionary.Values.FirstOrDefault();
        }

        public async Task<Livro> CreateAsync(Livro livro)
        {
            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            try
            {
                // Inserir o livro
                var livroQuery = @"
                    INSERT INTO Livro (Titulo, Editora, Edicao, AnoPublicacao)
                    OUTPUT INSERTED.Codi
                    VALUES (@Titulo, @Editora, @Edicao, @AnoPublicacao)";

                livro.Id = await _connection.ExecuteScalarAsync<int>(livroQuery, livro);

                // Inserir autores associados
                if (livro.Autores?.Any() == true)
                {
                    var autorQuery = "INSERT INTO Livro_Autor (Livro_Codi, Autor_CodAu) VALUES (@LivroId, @AutorId)";

                    foreach (var autor in livro.Autores)
                    {
                        await _connection.ExecuteAsync(autorQuery, new { LivroId = livro.Id, AutorId = autor.AutorId });
                    }
                }

                // Inserir assuntos associados
                if (livro.Assuntos?.Any() == true)
                {
                    var assuntoQuery = "INSERT INTO Livro_Assunto (Livro_Codi, Assunto_codAs) VALUES (@LivroId, @AssuntoId)";

                    foreach (var assunto in livro.Assuntos)
                    {
                        await _connection.ExecuteAsync(assuntoQuery, new { LivroId = livro.Id, AssuntoId = assunto.AssuntoId });
                    }
                }

                // Inserir preços associados
                if (livro.Precos?.Any() == true)
                {
                    var precoQuery = @"
                        INSERT INTO LivroPreco (Livro_Codi, TipoCompra_Id, Preco)
                        VALUES (@LivroId, @TipoCompraId, @Valor)";

                    foreach (var preco in livro.Precos)
                    {
                        await _connection.ExecuteAsync(precoQuery, new
                        {
                            LivroId = livro.Id,
                            TipoCompraId = preco.TipoCompraId,
                            Valor = preco.Valor
                        });
                    }
                }

                transaction.Complete();

                return livro;
            }
            catch
            {
                transaction.Dispose();

                throw;
            }
        }

        public async Task UpdateAsync(Livro livro)
        {
            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            try
            {
                // Atualizar o livro
                var livroQuery = @"
                    UPDATE Livro 
                    SET Titulo = @Titulo, 
                        Editora = @Editora, 
                        Edicao = @Edicao, 
                        AnoPublicacao = @AnoPublicacao
                    WHERE Codi = @Id";

                await _connection.ExecuteAsync(livroQuery, livro);

                // Remover associações antigas
                await _connection.ExecuteAsync("DELETE FROM Livro_Autor WHERE Livro_Codi = @Id", new { Id = livro.Id });
                await _connection.ExecuteAsync("DELETE FROM Livro_Assunto WHERE Livro_Codi = @Id", new { Id = livro.Id });
                await _connection.ExecuteAsync("DELETE FROM LivroPreco WHERE Livro_Codi = @Id", new { Id = livro.Id });

                transaction.Complete();
            }
            catch
            {
                transaction.Dispose();

                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            await _connection.ExecuteAsync("DELETE FROM Livro WHERE Codi = @Id", new { Id = id });
        }
    }
}