using System.ComponentModel.DataAnnotations;

namespace MyDotNetAPI.Models;

public class Pokemon
{
    [Key]
    public int Id  { get; set; }
    
    [StringLength(maximumLength: 100, MinimumLength = 2)]
    public required string Name { get; set; }
}