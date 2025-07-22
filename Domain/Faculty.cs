using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain;

public record Faculty
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("faculty_id")]
    [Key]
    public int Id { get; init; }
    
    public required string Name { get; init; }
    public required string ShortName { get; init; }

    public ICollection<Group> Groups { get; init; } = new List<Group>();
}