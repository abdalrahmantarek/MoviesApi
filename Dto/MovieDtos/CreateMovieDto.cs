namespace MoviesApi.Dto;
public class CreateMovieDto : MovieDto
{
  public IFormFile Poseter { get; set; }

}