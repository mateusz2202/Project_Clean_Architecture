using Identity.Application.Common.Interfaces;
using Identity.Shared.Contracts;
using Identity.Shared.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure;

public class IndentityDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    
    private readonly ICurrentUserService _currentUserService;

    public IndentityDbContext(DbContextOptions<IndentityDbContext> options, ICurrentUserService currentUserService)
        : base(options)
    {
        _currentUserService = currentUserService;
    }
   

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries<IAuditableEntity>().ToList())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedOn = DateTime.UtcNow;
                    entry.Entity.CreatedBy = _currentUserService?.UserId ?? string.Empty;
                    entry.Entity.ModifiedOn = DateTime.UtcNow;
                    entry.Entity.ModifiedBy = _currentUserService?.UserId ?? string.Empty;
                    break;
                case EntityState.Modified:
                    entry.Entity.ModifiedOn = DateTime.UtcNow;
                    entry.Entity.ModifiedBy = _currentUserService?.UserId ?? string.Empty;
                    break;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{

    //    string connectionString = "Server=Hermes\\HERMES_SERVER;Database=AA_Indentity;User ID=sa;Password=997997;TrustServerCertificate=True;Trusted_Connection=True;";
    //    optionsBuilder.UseSqlServer(connectionString);

    //}
}
