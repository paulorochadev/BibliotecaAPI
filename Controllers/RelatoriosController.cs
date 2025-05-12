using BibliotecaAPI.Models.DTOs;
using BibliotecaAPI.Repositories;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BibliotecaAPI.Controllers
{
    /// <summary>
    /// Controller para Geração de Relatórios
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RelatoriosController : ControllerBase
    {
        private readonly IDbConnection _connection;
        private readonly ILogger<RelatoriosController> _logger;

        public RelatoriosController(IDbConnection connection, ILogger<RelatoriosController> logger)
        {
            _connection = connection;
            _logger = logger;
        }

        /// <summary>
        /// Obtém o relatório de livros agrupados por autor
        /// </summary>
        /// <returns>Lista de registros do relatório</returns>
        /// <response code="200">Retorna o relatório com sucesso</response>
        /// <response code="500">Erro interno ao processar o relatório</response>
        [HttpGet("livros-por-autor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<RelatorioLivrosPorAutorDTO>>> GetLivrosPorAutor()
        {
            try
            {
                _logger.LogInformation("Iniciando geração do relatório de livros por autor");

                var query = "SELECT * FROM vw_RelatorioLivrosPorAutor";
                var result = await _connection.QueryAsync<RelatorioLivrosPorAutorDTO>(query);

                _logger.LogInformation($"Relatório gerado com {result.Count()} registros");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao gerar relatório de livros por autor");

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um erro ao processar o relatório. Por favor, tente novamente mais tarde.");
            }
        }
    }
}