using AutoMapper;
using BibliotecaAPI.Models.DTOs;
using BibliotecaAPI.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutoresController : ControllerBase
    {
        private readonly IAutorService _autorService;
        private readonly IMapper _mapper;

        public AutoresController(IAutorService autorService, IMapper mapper)
        {
            _autorService = autorService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AutorDTO>>> GetAutores()
        {
            var autores = await _autorService.GetAllAutoresAsync();

            return Ok(autores);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AutorDTO>> GetAutor(int id)
        {
            var autor = await _autorService.GetAutorByIdAsync(id);

            if (autor == null)
            {
                return NotFound();
            }

            return autor;
        }

        [HttpPost]
        public async Task<ActionResult<AutorDTO>> PostAutor(AutorDTO autorDto)
        {
            var createdAutor = await _autorService.CreateAutorAsync(autorDto);

            return CreatedAtAction(nameof(GetAutor), new { id = createdAutor.Id }, createdAutor);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAutor(int id, AutorDTO autorDto)
        {
            if (id != autorDto.Id)
            {
                return BadRequest();
            }

            try
            {
                await _autorService.UpdateAutorAsync(id, autorDto);

                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAutor(int id)
        {
            try
            {
                await _autorService.DeleteAutorAsync(id);

                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}