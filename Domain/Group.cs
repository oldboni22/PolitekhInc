using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain;

public record Group()
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("group_id")]
    [Key]
    public int Id { get; init; }
    
    public required string Number { get; init; }

    [ForeignKey(nameof(Faculty))]
    public int FacultyId { get; init; }
    
    public Faculty Faculty { get; init; } = null!;
    
    public ICollection<Student> Students { get; init; } = new List<Student>();
}