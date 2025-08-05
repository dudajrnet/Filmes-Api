using AutoMapper;
using FilmesApi.Data;
using FilmesApi.Data.Dtos;
using FilmesApi.Models;
using Microsoft.AspNetCore.JsonPatch;
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

        /// <summary>
        /// Adiciona um filme ao banco de dados
        /// </summary>
        /// <param name="filmeDto">Objeto com os campos necessários para criação de um filme</param>
        /// <returns>IActionResult</returns>
        /// <response code="201">Caso inserção seja feita com sucesso</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [HttpPost]        
        public IActionResult AdicionarFilme([FromBody] CreateFilmeDto filmeDto)
        {
            var filme = _mapper.Map<Filme>(filmeDto);

            _context.Filmes.Add(filme);
            _context.SaveChanges();

            return CreatedAtAction(nameof(RecuperarFilmePorId),
                new { id = filme.Id },
                filme);
        }

        /// <summary>
        /// Lista todos os filmes do banco de dados.
        /// </summary>        
        /// <returns>IEnumerable</returns>
        /// <response code="200">Caso a requisição seja feita com sucesso.j</response>        
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public IEnumerable<ReadFilmeDto> RecuperarFilmes(int skip = 0, int take = 10)
        {
            return _mapper.Map<IEnumerable<ReadFilmeDto>>(_context.Filmes.Skip(skip).Take(take));
        }

        /// <summary>
        /// Retorna o filme pesquisado pelo Id
        /// </summary>        
        /// <returns>IActionResult</returns>
        /// <response code="200">Caso a requisição seja feita com sucesso.j</response>        
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{Id}")]
        public IActionResult RecuperarFilmePorId(int Id)
        {
            var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == Id);

            if (filme == null) return NotFound();

            var filmeDto = _mapper.Map<ReadFilmeDto>(filme);

            return Ok(filmeDto);
        }

        /// <summary>
        /// Atualiza o filme pesquisando pelo Id
        /// </summary>        
        /// <returns>IActionResult</returns>
        /// <response code="204">Caso a requisição seja feita com sucesso.j</response>        
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPut("{id}")]
        public IActionResult AtualizarFilme(int id, [FromBody] UpdateFilmeDto filmeDto)
        {

            var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
            
            if (filme == null) return NotFound();
            _mapper.Map(filmeDto, filme);            
            _context.SaveChanges();

            return NoContent();
        }

        /// <summary>
        /// Atualiza campos específicos de um filme pesquisando pelo Id
        /// </summary>        
        /// <returns>IActionResult</returns>
        /// <response code="204">Caso a requisição seja feita com sucesso.j</response>        
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPatch("{id}")]
        public IActionResult AtualizarFilmeParcial(int id, JsonPatchDocument<UpdateFilmeDto> patch)
        {

            var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);

            if (filme == null) return NotFound();

            var filmeParaAtualizar = _mapper.Map<UpdateFilmeDto>(filme);
            
            patch.ApplyTo(filmeParaAtualizar, ModelState);

            if (!TryValidateModel(filmeParaAtualizar))
                return ValidationProblem();

            _mapper.Map(filmeParaAtualizar, filme);          
            _context.SaveChanges();

            return NoContent();
        }

        /// <summary>
        /// Apaga um filme pesquisando pelo Id
        /// </summary>        
        /// <returns>IActionResult</returns>
        /// <response code="204">Caso a requisição seja feita com sucesso.j</response>        
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpDelete("{Id}")]
        public IActionResult DeletarFilme(int Id)
        {
            var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == Id);

            if (filme == null) return NotFound();

            _context.Remove(filme);
            _context.SaveChanges();
            
            return NoContent();
        }

    }
}
