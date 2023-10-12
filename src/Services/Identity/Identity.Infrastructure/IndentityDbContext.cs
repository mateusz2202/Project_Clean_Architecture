using Identity.Shared.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure;

public class IndentityDbContext : IdentityDbContext<ApplicationUser,ApplicationRole,string>
{
    public IndentityDbContext()
    {

    }

    public IndentityDbContext(DbContextOptions<IndentityDbContext> options) : base(options)
    {
    }
}
