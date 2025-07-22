using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain;

public record Subject
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("subject_id")]
    [Key]
    public int Id { get; init; }
    
    public required string Name { get; init; }

    public ICollection<Teacher> Teachers { get; init; } = new List<Teacher>();
}