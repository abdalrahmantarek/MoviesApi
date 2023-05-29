using MoviesApi.GenresService;

namespace MoviesApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GenresController : ControllerBase
{
  private readonly IGenresService _genresService;
  public GenresController(IGenresService genresService)
  {
    _genresService = genresService;
  }


  [HttpGet]
  public async Task<IActionResult> GetAllData()
  {
    var genres = await _genresService.GetAll();

    if(genres != null) return Ok(genres);
    
    return NotFound();
  }  

  [HttpPost]
  public async Task<IActionResult> AddGenre([FromBody] GenreDto dto)
  {
      var genre = new Genre() {Name = dto.Name};
      await _genresService.Add(genre);
      return Ok(genre);
  }

  [HttpPut("{id}")]
  public async Task<IActionResult> UpdateGenre(byte id, [FromBody] GenreDto dto)
  {
    var genre = await _genresService.GetById(id);
    if(genre is null) return NotFound($"There is no genre with this id: {id}");
    
    genre.Name = dto.Name;
    _genresService.Update(genre);

    return Ok(genre);
  
  }

  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteGenre(byte id)
  {
    var genre = await _genresService.GetById(id);
    if(genre is null) return NotFound($"There is no genre with this id: {id}");
    
    _genresService.Delete(genre);

    return Ok(genre);

  }
}
