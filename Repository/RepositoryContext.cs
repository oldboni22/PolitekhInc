using Domain;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class RepositoryContext(DbContextOptions options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Teacher>().
            HasMany(teach => teach.Subjects).
            WithMany(sub => sub.Teachers);
        
        modelBuilder.Entity<Group>().
            HasOne(g => g.Faculty).
            WithMany(fac => fac.Groups).
            HasForeignKey(group => group.FacultyId).
            IsRequired();

        modelBuilder.Entity<Student>().
            HasOne(stud => stud.Group).
            WithMany(group => group.Students).
            HasForeignKey(stud => stud.GroupId).
            IsRequired();
    }
    
    public DbSet<Student> Students { get; init; }
    public DbSet<Group> Groups { get; init; }
    public DbSet<Faculty> Faculties { get; init; }
    
    public DbSet<Teacher> Teachers { get; init; }
    public DbSet<Subject> Subjects { get; init; }
}