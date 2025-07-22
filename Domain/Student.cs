using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain;

public record Student
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("student_id")]
    [Key]
    public int Id { get; init; }
    
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    
    [ForeignKey(nameof(Group))]
    public int GroupId { get; init;}
    
    public required Group Group { get; init; }

}