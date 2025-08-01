using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Repository;

namespace PolitekhInc.DbContextFactories;

public class RepositoryContextFactory : IDesignTimeDbContextFactory<RepositoryContext>
{
    public RepositoryContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder().
            SetBasePath(Directory.GetCurrentDirectory()).
            AddJsonFile("appsettings.json").
            Build();

        var builder = new DbContextOptionsBuilder().
            UseSqlServer(config.GetConnectionString("sqlCon"),
                options => options.MigrationsAssembly("PolitekhInc"));

        return new RepositoryContext(builder.Options);
    }
}