using AutoMapper;
using BibliotecaAPI.Models.DTOs;
using BibliotecaAPI.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaAPI.Controllers
{
    /// <summary>
    /// Controller para Gerenciamento de Autores
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AutoresController : ControllerBase
    {
        private readonly IAutorService _autorService;
        private readonly IMapper _mapper;
        private readonly ILogger<AutoresController> _logger;

        public AutoresController(IAutorService autorService, IMapper mapper, ILogger<AutoresController> logger)
        {
            _autorService = autorService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Obtém todos os Autores
        /// </summary>
        /// <returns>Lista de autores</returns>
        /// <response code="200">Retorna a lista de autores</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<AutorDTO>>> GetAutores()
        {
            _logger.LogInformation("Obter todos os autores");

            var autores = await _autorService.GetAllAutoresAsync();

            return Ok(autores);
        }

        /// <summary>
        /// Obtém um Autor específico pelo ID
        /// </summary>
        /// <param name="id">ID do autor</param>
        /// <returns>Dados do autor</returns>
        /// <response code="200">Retorna o autor solicitado</response>
        /// <response code="404">Autor não encontrado</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AutorDTO>> GetAutor(int id)
        {
            _logger.LogInformation("Obter autor por ID: {Id}", id);

            var autor = await _autorService.GetAutorByIdAsync(id);

            if (autor == null)
            {
                _logger.LogWarning("Autor com ID {Id} não encontrado", id);

                return NotFound();
            }

            return autor;
        }

        /// <summary>
        /// Cria um novo Autor
        /// </summary>
        /// <param name="autorDto">Dados do autor</param>
        /// <returns>Autor criado</returns>
        /// <response code="201">Autor criado com sucesso</response>
        /// <response code="400">Dados inválidos</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AutorDTO>> PostAutor(AutorDTO autorDto)
        {
            _logger.LogInformation("Criar novo autor: {@Autor}", autorDto);

            try
            {
                var createdAutor = await _autorService.CreateAutorAsync(autorDto);

                _logger.LogInformation("Autor criado com ID: {Id}", createdAutor.Id);

                return CreatedAtAction(nameof(GetAutor), new { id = createdAutor.Id }, createdAutor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar autor");

                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Atualiza um Autor existente
        /// </summary>
        /// <param name="id">ID do autor</param>
        /// <param name="autorDto">Dados atualizados do autor</param>
        /// <returns>Sem conteúdo</returns>
        /// <response code="204">Autor atualizado com sucesso</response>
        /// <response code="400">IDs não correspondem ou dados inválidos</response>
        /// <response code="404">Autor não encontrado</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutAutor(int id, AutorDTO autorDto)
        {
            _logger.LogInformation("Atualizar autor ID: {Id} com dados: {@Autor}", id, autorDto);

            if (id != autorDto.Id)
            {
                _logger.LogWarning("IDs não correspondem - Rota: {Id}, DTO: {DtoId}", id, autorDto.Id);

                return BadRequest();
            }

            try
            {
                await _autorService.UpdateAutorAsync(id, autorDto);

                _logger.LogInformation("Autor ID: {Id} atualizado com sucesso", id);

                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Autor não encontrado - ID: {Id}", id);

                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar autor ID: {Id}", id);

                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Remove um Autor
        /// </summary>
        /// <param name="id">ID do autor</param>
        /// <returns>Sem conteúdo</returns>
        /// <response code="204">Autor removido com sucesso</response>
        /// <response code="404">Autor não encontrado</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAutor(int id)
        {
            _logger.LogInformation("Excluir autor ID: {Id}", id);

            try
            {
                await _autorService.DeleteAutorAsync(id);

                _logger.LogInformation("Autor ID: {Id} excluído com sucesso", id);

                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Autor não encontrado - ID: {Id}", id);

                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir autor ID: {Id}", id);

                return BadRequest(ex.Message);
            }
        }
    }
}