using MoviesApi.MoviesService;
using MoviesApi.GenresService;

namespace MoviesApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MoviesController : ControllerBase
{
  private readonly IMoviesService _moviesService;
  private readonly IGenresService _genresService;
  private string[] _allowedExtentions = {".png", ".jpg"};
  private int _allowedMaxSize = 1048576;
  public MoviesController(IMoviesService moviesService, IGenresService genresService)
  {
    _moviesService = moviesService;
    _genresService = genresService;
  }

  [HttpGet]
  public async Task<IActionResult> GetAll()
  {
    var movies = await _moviesService.GetAll();
    return Ok(movies);
  }

  [HttpGet("GetByMovieId/{movieId}")]
  public async Task<IActionResult> GetByMovieId(int movieId)
  {
    var movie = await _moviesService.GetById(movieId);
    if(movie is null)
      return NotFound($"There is no movie with this id: {movieId}");
    
    return Ok(movie);

  }
  [HttpGet("GetByGenreId/{genreId}")]
  public async Task<IActionResult> GetByGenreId(byte genreId)
  {
    var movie = await _moviesService.GetAll(genreId);
    if(movie is null)
      return NotFound($"There are no movies with this id: {genreId}");
    return Ok(movie);
  }
  [HttpPost]
  public async Task<IActionResult> Add([FromForm] CreateMovieDto dto)
  {
    if(!_allowedExtentions.Contains(Path.GetExtension(dto.Poseter.FileName).ToLower()))
      return BadRequest("Only png or jpg images are allowed");
    if(dto.Poseter.Length > _allowedMaxSize)
      return BadRequest("Max allowed size is 1MB");

    var isValidGenreId = await _genresService.IsValidGenre(dto.GenreId);
    
    if(!isValidGenreId)
      return BadRequest("Invalid genre Id");
    using var dataStream = new MemoryStream();
    await dto.Poseter.CopyToAsync(dataStream);
    
    var movie = new Movie
    {
      GenreId = dto.GenreId,
      Title = dto.Title,
      Poseter = dataStream.ToArray(),
      Rate = dto.Rate,
      Year = dto.Year,
      Storeline = dto.Storeline
    };
    
    _moviesService.Create(movie);
    return Ok(movie);
  }

  [HttpPut("{id}")]
  public async Task<IActionResult> UpdateById(int id, [FromForm] UpdateMovieDto dto)
  {
    
    var movie = await _moviesService.GetById(id);
    if(movie is null) return NotFound($"There is no movies with this id:{id}");
    
    var isValidGenreId = await _genresService.IsValidGenre(dto.GenreId); 
    if(!isValidGenreId) return BadRequest("Invaild genre Id");
    

    if(dto.Poseter is not null)
    {
      if(!_allowedExtentions.Contains(Path.GetExtension(dto.Poseter.FileName).ToLower())) return BadRequest("Only .png or .jpg images ara allowed");
      if(dto.Poseter.Length > _allowedMaxSize) return BadRequest("Max size allowed is 1MB");
      
      using var dataStream = new  MemoryStream();
      await dto.Poseter.CopyToAsync(dataStream);
      
      movie.Poseter = dataStream.ToArray();
    }
    //Update data
    movie.Title = dto.Title;
    movie.Rate = dto.Rate;
    movie.Storeline = dto.Storeline;
    movie.Year = dto.Year;
    movie.GenreId = dto.GenreId;
    
    _moviesService.Update(movie);

    return Ok(movie);

  }

  [HttpDelete("MovieId/{id}")]
  public async Task<IActionResult> DelteById(int id)
  {
    var movie = await _moviesService.GetById(id);
    if(movie is null)
      return NotFound($"There is no movie with this id: {id}");
    
    _moviesService.Delete(movie);
    
    return Ok(movie);
  } 
}