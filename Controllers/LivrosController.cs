using AutoMapper;
using BibliotecaAPI.Models.DTOs;
using BibliotecaAPI.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaAPI.Controllers
{
    /// <summary>
    /// Controller para Gerenciamento de Livros
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LivrosController : ControllerBase
    {
        private readonly ILivroService _livroService;
        private readonly IMapper _mapper;
        private readonly ILogger<LivrosController> _logger;

        public LivrosController(ILivroService livroService, IMapper mapper, ILogger<LivrosController> logger)
        {
            _livroService = livroService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Obtém todos os Livros
        /// </summary>
        /// <returns>Lista de livros</returns>
        /// <response code="200">Retorna a lista de livros</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<LivroDTO>>> GetLivros()
        {
            _logger.LogInformation("Obter todos os livros");

            var livros = await _livroService.GetAllLivrosAsync();

            return Ok(livros);
        }

        /// <summary>
        /// Obtém um Livro específico pelo ID
        /// </summary>
        /// <param name="id">ID do livro</param>
        /// <returns>Dados do livro</returns>
        /// <response code="200">Retorna o livro solicitado</response>
        /// <response code="404">Livro não encontrado</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LivroDTO>> GetLivro(int id)
        {
            _logger.LogInformation("Obter livro por ID: {Id}", id);

            var livro = await _livroService.GetLivroByIdAsync(id);

            if (livro == null)
            {
                _logger.LogWarning("Livro com ID {Id} não encontrado", id);

                return NotFound();
            }

            return Ok(livro);
        }

        /// <summary>
        /// Cria um novo Livro
        /// </summary>
        /// <param name="livroDto">Dados do livro</param>
        /// <returns>Livro criado</returns>
        /// <response code="201">Livro criado com sucesso</response>
        /// <response code="400">Dados inválidos</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LivroDTO>> PostLivro(LivroDTO livroDto)
        {
            _logger.LogInformation("Criar novo livro: {@Livro}", livroDto);

            try
            {
                var createdLivro = await _livroService.CreateLivroAsync(livroDto);

                _logger.LogInformation("Livro criado com ID: {Id}", createdLivro.Id);

                return CreatedAtAction(nameof(GetLivro), new { id = createdLivro.Id }, createdLivro);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar livro");

                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Atualiza um Livro existente
        /// </summary>
        /// <param name="id">ID do livro</param>
        /// <param name="livroDto">Dados atualizados do livro</param>
        /// <returns>Sem conteúdo</returns>
        /// <response code="204">Livro atualizado com sucesso</response>
        /// <response code="400">IDs não correspondem ou dados inválidos</response>
        /// <response code="404">Livro não encontrado</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutLivro(int id, LivroDTO livroDto)
        {
            _logger.LogInformation("Atualizar livro ID: {Id} com dados: {@Livro}", id, livroDto);

            if (id != livroDto.Id)
            {
                _logger.LogWarning("IDs não correspondem - Rota: {Id}, DTO: {DtoId}", id, livroDto.Id);

                return BadRequest();
            }

            try
            {
                await _livroService.UpdateLivroAsync(id, livroDto);

                _logger.LogInformation("Livro ID: {Id} atualizado com sucesso", id);

                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Livro não encontrado - ID: {Id}", id);

                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar livro ID: {Id}", id);

                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Remove um Livro
        /// </summary>
        /// <param name="id">ID do livro</param>
        /// <returns>Sem conteúdo</returns>
        /// <response code="204">Livro removido com sucesso</response>
        /// <response code="404">Livro não encontrado</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteLivro(int id)
        {
            _logger.LogInformation("Excluir livro ID: {Id}", id);

            try
            {
                await _livroService.DeleteLivroAsync(id);

                _logger.LogInformation("Livro ID: {Id} excluído com sucesso", id);

                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Livro não encontrado - ID: {Id}", id);

                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir livro ID: {Id}", id);

                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Obtém os preços de um Livro
        /// </summary>
        /// <param name="livroId">ID do livro</param>
        /// <returns>Lista de preços</returns>
        /// <response code="200">Retorna a lista de preços</response>
        /// <response code="404">Livro não encontrado</response>
        [HttpGet("{livroId}/precos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<PrecoLivroDTO>>> GetPrecosByLivro(int livroId)
        {
            _logger.LogInformation("Obter preços do livro ID: {Id}", livroId);

            try
            {
                var precos = await _livroService.GetPrecosByLivroIdAsync(livroId);

                return Ok(precos);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Livro não encontrado - ID: {Id}", livroId);

                return NotFound();
            }
        }

        /// <summary>
        /// Adiciona um preço a um Livro
        /// </summary>
        /// <param name="livroId">ID do livro</param>
        /// <param name="precoDto">Dados do preço</param>
        /// <returns>Preço criado</returns>
        /// <response code="201">Preço adicionado com sucesso</response>
        /// <response code="400">Dados inválidos</response>
        /// <response code="404">Livro não encontrado</response>
        [HttpPost("{livroId}/precos")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PrecoLivroDTO>> AddPrecoToLivro(int livroId, PrecoLivroDTO precoDto)
        {
            _logger.LogInformation("Adicionar preço ao livro ID: {Id}, Preço: {@Preco}", livroId, precoDto);

            try
            {
                var createdPreco = await _livroService.AddPrecoToLivroAsync(livroId, precoDto);

                _logger.LogInformation("Preço ID: {Id} adicionado ao livro ID: {LivroId}", createdPreco.Id, livroId);

                return CreatedAtAction(nameof(GetPrecosByLivro), new { livroId = livroId }, createdPreco);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Livro não encontrado - ID: {Id}", livroId);

                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar preço ao livro ID: {Id}", livroId);

                return BadRequest(ex.Message);
            }
        }
    }
}