namespace MoviesApi.Dto;

public class MovieDto
{
  [MaxLength(250)]
  public string Title {get;set;}
  public int Year {get;set;}
  public double Rate {get;set;}
  [MaxLength(2500)]
  public string Storeline {get;set;}
  //Foreign Key
  public byte GenreId {get;set;}


}