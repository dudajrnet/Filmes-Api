using AutoMapper;
using FilmesApi.Data;
using FilmesApi.Data.Dtos;
using FilmesApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace FilmesApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FilmeController : ControllerBase
    {
        private FilmeContext _context;
        private IMapper _mapper;

        public FilmeController(FilmeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult AdicionarFilme([FromBody] CreateFilmeDto filmeDto)
        {
            var filme = _mapper.Map<Filme>(filmeDto);

            _context.Filmes.Add(filme);
            _context.SaveChanges();

            return CreatedAtAction(nameof(RecuperarFilmesPorId),
                new { id = filme.Id },
                filme);
        }

        [HttpGet]
        public IEnumerable<Filme> RecuperarFilmes(int skip = 0, int take = 10)
        {
            return _context.Filmes.Skip(skip).Take(take);
        }

        [HttpGet("{Id}")]
        public IActionResult RecuperarFilmesPorId(int Id)
        {
            var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == Id);

            if (filme == null) return NotFound();

            return Ok(filme);
        }
    }
}
