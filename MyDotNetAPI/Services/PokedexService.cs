using MyDotNetAPI.Data;
using MyDotNetAPI.Models;

namespace MyDotNetAPI.Services;

public interface IPokedexService
{
    IQueryable<Pokemon> List();
}

public class PokedexService: IPokedexService
{
    private readonly MyDbContextSqLite _dbContext;
    
    public PokedexService(MyDbContextSqLite dbContext)
    {
        _dbContext = dbContext;
    }
    
    public IQueryable<Pokemon> List()
    {
        // try
        // {
            return _dbContext.Pokedex.OrderBy(columns => columns.Id).Skip(0).Take(5);
        // }
        // catch (Exception ex)
        // {
            // return new Queryable>();
        // }
    }
}