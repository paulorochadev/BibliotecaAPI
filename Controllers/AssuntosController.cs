using AutoMapper;
using BibliotecaAPI.Models.DTOs;
using BibliotecaAPI.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssuntosController : ControllerBase
    {
        private readonly IAssuntoService _assuntoService;
        private readonly IMapper _mapper;

        public AssuntosController(IAssuntoService assuntoService, IMapper mapper)
        {
            _assuntoService = assuntoService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AssuntoDTO>>> GetAssuntos()
        {
            var assuntos = await _assuntoService.GetAllAssuntosAsync();

            return Ok(assuntos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AssuntoDTO>> GetAssunto(int id)
        {
            var assunto = await _assuntoService.GetAssuntoByIdAsync(id);

            if (assunto == null)
            {
                return NotFound();
            }

            return assunto;
        }

        [HttpPost]
        public async Task<ActionResult<AssuntoDTO>> PostAssunto(AssuntoDTO assuntoDto)
        {
            var createdAssunto = await _assuntoService.CreateAssuntoAsync(assuntoDto);

            return CreatedAtAction(nameof(GetAssunto), new { id = createdAssunto.Id }, createdAssunto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAssunto(int id, AssuntoDTO assuntoDto)
        {
            if (id != assuntoDto.Id)
            {
                return BadRequest();
            }

            try
            {
                await _assuntoService.UpdateAssuntoAsync(id, assuntoDto);

                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAssunto(int id)
        {
            try
            {
                await _assuntoService.DeleteAssuntoAsync(id);

                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}