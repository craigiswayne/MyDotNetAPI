using Microsoft.EntityFrameworkCore;
using MyDotNetAPI.Models;

namespace MyDotNetAPI.Data;

public class MyDbContextSqLite : DbContext
{
    public MyDbContextSqLite(DbContextOptions<MyDbContextSqLite> options) : base(options)
    {
    }

    public DbSet<Pokemon> Pokedex { get; set; }

}