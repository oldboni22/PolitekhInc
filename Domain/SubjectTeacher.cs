namespace Domain;

public record SubjectTeacher
{
    public int TeacherId { get; init; }
    public int SubjectId { get; init; }

    public Teacher Teacher { get; init; } = null!;
    public Subject Subject { get; init; } = null!;
}