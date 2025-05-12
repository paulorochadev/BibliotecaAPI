using AutoMapper;
using BibliotecaAPI.Models.DTOs;
using BibliotecaAPI.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaAPI.Controllers
{
    /// <summary>
    /// Controller para Gerenciamento de Assuntos
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AssuntosController : ControllerBase
    {
        private readonly IAssuntoService _assuntoService;
        private readonly IMapper _mapper;
        private readonly ILogger<AssuntosController> _logger;

        public AssuntosController(IAssuntoService assuntoService, IMapper mapper, ILogger<AssuntosController> logger)
        {
            _assuntoService = assuntoService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Obtém todos os Assuntos
        /// </summary>
        /// <returns>Lista de assuntos</returns>
        /// <response code="200">Retorna a lista de assuntos</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<AssuntoDTO>>> GetAssuntos()
        {
            _logger.LogInformation("Obter todos os assuntos");

            var assuntos = await _assuntoService.GetAllAssuntosAsync();

            return Ok(assuntos);
        }

        /// <summary>
        /// Obtém um Assunto específico pelo ID
        /// </summary>
        /// <param name="id">ID do assunto</param>
        /// <returns>Dados do assunto</returns>
        /// <response code="200">Retorna o assunto solicitado</response>
        /// <response code="404">Assunto não encontrado</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AssuntoDTO>> GetAssunto(int id)
        {
            _logger.LogInformation("Obter assunto por ID: {Id}", id);

            var assunto = await _assuntoService.GetAssuntoByIdAsync(id);

            if (assunto == null)
            {
                _logger.LogWarning("Assunto com ID {Id} não encontrado", id);

                return NotFound();
            }

            return Ok(assunto);
        }

        /// <summary>
        /// Cria um novo Assunto
        /// </summary>
        /// <param name="assuntoDto">Dados do assunto</param>
        /// <returns>Assunto criado</returns>
        /// <response code="201">Assunto criado com sucesso</response>
        /// <response code="400">Dados inválidos</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AssuntoDTO>> PostAssunto(AssuntoDTO assuntoDto)
        {
            _logger.LogInformation("Criar novo assunto: {@Assunto}", assuntoDto);

            try
            {
                var createdAssunto = await _assuntoService.CreateAssuntoAsync(assuntoDto);

                _logger.LogInformation("Assunto criado com ID: {Id}", createdAssunto.Id);

                return CreatedAtAction(nameof(GetAssunto), new { id = createdAssunto.Id }, createdAssunto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar assunto");

                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Atualiza um Assunto existente
        /// </summary>
        /// <param name="id">ID do assunto</param>
        /// <param name="assuntoDto">Dados atualizados do assunto</param>
        /// <returns>Sem conteúdo</returns>
        /// <response code="204">Assunto atualizado com sucesso</response>
        /// <response code="400">IDs não correspondem ou dados inválidos</response>
        /// <response code="404">Assunto não encontrado</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutAssunto(int id, AssuntoDTO assuntoDto)
        {
            _logger.LogInformation("Atualizar assunto ID: {Id} com dados: {@Assunto}", id, assuntoDto);

            if (id != assuntoDto.Id)
            {
                _logger.LogWarning("IDs não correspondem - Rota: {Id}, DTO: {DtoId}", id, assuntoDto.Id);

                return BadRequest();
            }

            try
            {
                await _assuntoService.UpdateAssuntoAsync(id, assuntoDto);

                _logger.LogInformation("Assunto ID: {Id} atualizado com sucesso", id);

                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Assunto não encontrado - ID: {Id}", id);

                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar assunto ID: {Id}", id);

                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Remove um Assunto
        /// </summary>
        /// <param name="id">ID do assunto</param>
        /// <returns>Sem conteúdo</returns>
        /// <response code="204">Assunto removido com sucesso</response>
        /// <response code="404">Assunto não encontrado</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAssunto(int id)
        {
            _logger.LogInformation("Excluir assunto ID: {Id}", id);

            try
            {
                await _assuntoService.DeleteAssuntoAsync(id);

                _logger.LogInformation("Assunto ID: {Id} excluído com sucesso", id);

                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Assunto não encontrado - ID: {Id}", id);

                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir assunto ID: {Id}", id);

                return BadRequest(ex.Message);
            }
        }
    }
}