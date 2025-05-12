using AutoMapper;
using BibliotecaAPI.Models.DTOs;
using BibliotecaAPI.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LivrosController : ControllerBase
    {
        private readonly ILivroService _livroService;
        private readonly IMapper _mapper;

        public LivrosController(ILivroService livroService, IMapper mapper)
        {
            _livroService = livroService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LivroDTO>>> GetLivros()
        {
            var livros = await _livroService.GetAllLivrosAsync();

            return Ok(livros);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LivroDTO>> GetLivro(int id)
        {
            var livro = await _livroService.GetLivroByIdAsync(id);

            if (livro == null)
            {
                return NotFound();
            }

            return livro;
        }

        [HttpPost]
        public async Task<ActionResult<LivroDTO>> PostLivro(LivroDTO livroDto)
        {
            try
            {
                var createdLivro = await _livroService.CreateLivroAsync(livroDto);

                return CreatedAtAction(nameof(GetLivro), new { id = createdLivro.Id }, createdLivro);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutLivro(int id, LivroDTO livroDto)
        {
            if (id != livroDto.Id)
            {
                return BadRequest();
            }

            try
            {
                await _livroService.UpdateLivroAsync(id, livroDto);

                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLivro(int id)
        {
            try
            {
                await _livroService.DeleteLivroAsync(id);

                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{livroId}/precos")]
        public async Task<ActionResult<IEnumerable<PrecoLivroDTO>>> GetPrecosByLivro(int livroId)
        {
            try
            {
                var precos = await _livroService.GetPrecosByLivroIdAsync(livroId);

                return Ok(precos);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("{livroId}/precos")]
        public async Task<ActionResult<PrecoLivroDTO>> AddPrecoToLivro(int livroId, PrecoLivroDTO precoDto)
        {
            try
            {
                var createdPreco = await _livroService.AddPrecoToLivroAsync(livroId, precoDto);

                return CreatedAtAction(nameof(GetPrecosByLivro), new { livroId = livroId }, createdPreco);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}