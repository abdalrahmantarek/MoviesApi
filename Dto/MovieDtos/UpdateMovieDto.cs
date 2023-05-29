namespace MoviesApi.Dto;

public class UpdateMovieDto : MovieDto
{
  public IFormFile? Poseter { get; set; }

}