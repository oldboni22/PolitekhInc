using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain;

public record Teacher
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("teacher_id")]
    [Key]
    public int Id { get; init; }
    
    public required string FirstName { get; init; }
    public required string LastName { get; init; }

    [ForeignKey(nameof(CuratedGroup))]
    public int? CuratedGroupId { get; init; }
    public Group? CuratedGroup { get; init; }

    public ICollection<Subject> Subjects = new List<Subject>();
}