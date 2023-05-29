namespace MoviesApi.GenresService;

public interface IGenresService
{
  Task<IEnumerable<Genre>> GetAll();
  Task<Genre> GetById(byte id);
  Task<Genre> Add(Genre genre);
  Task<bool> IsValidGenre(byte id);
  Genre Update(Genre genre);
  Genre Delete(Genre genre);

}