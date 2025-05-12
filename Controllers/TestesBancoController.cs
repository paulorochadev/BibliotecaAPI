using BibliotecaAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BibliotecaAPI.Controllers
{
    /// <summary>
    /// Controller para Testes do Banco de Dados
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TestesBancoController : ControllerBase
    {
        private readonly string _connectionString;
        private readonly ILogger<TestesBancoController> _logger;

        public TestesBancoController(IConfiguration configuration, ILogger<TestesBancoController> logger)
        {
            _connectionString = configuration.GetConnectionString("BibliotecaConnection");
            _logger = logger;
        }

        /// <summary>
        /// Testa a Conexão com o Banco de Dados
        /// </summary>
        /// <returns>Resultado do teste de conexão</returns>
        /// <response code="200">Conexão bem-sucedida</response>
        /// <response code="500">Erro na conexão</response>
        [HttpGet("test-db")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> TestDatabaseConnection()
        {
            try
            {
                _logger.LogInformation("Testando conexão com o banco de dados");

                await using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();
                var canConnect = connection.State == ConnectionState.Open;
                await connection.CloseAsync();

                _logger.LogInformation("Conexão com o banco de dados: {Status}", canConnect ? "OK" : "Falhou");

                return Ok(new
                {
                    Success = canConnect,
                    Message = canConnect ?
                        "Conexão com o banco de dados estabelecida com sucesso" :
                        "Não foi possível estabelecer conexão com o banco de dados"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao testar conexão com o banco de dados");

                return StatusCode(500, new
                {
                    Success = false,
                    Message = "Erro ao conectar com o banco de dados",
                    Error = ex.Message
                });
            }
        }
    }
}