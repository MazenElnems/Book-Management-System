using BMS.API.Constants;
using BMS.API.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BMS.API.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options):
        base(options)
    {
        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.UseSeeding(async (db, _) =>
        {
            if (db.Set<IdentityRole<Guid>>().FirstOrDefault(r => r.Name == ApplicationRoles.Admin) is null)
            {
                db.Set<IdentityRole<Guid>>().Add(new IdentityRole<Guid> { Name = ApplicationRoles.Admin , NormalizedName = ApplicationRoles.Admin.ToUpper()});
                db.SaveChanges();
            }
            if (db.Set<IdentityRole<Guid>>().FirstOrDefault(r => r.Name == ApplicationRoles.User) is null)
            {
                db.Set<IdentityRole<Guid>>().Add(new IdentityRole<Guid> { Name = ApplicationRoles.User , NormalizedName = ApplicationRoles.User.ToUpper() });
                db.SaveChanges();
            }
        })
            .UseAsyncSeeding(async (db, _, cancellationToken) =>
            {
                if (await db.Set<IdentityRole<Guid>>().FirstOrDefaultAsync(r => r.Name == ApplicationRoles.Admin) is null)
                {
                    db.Set<IdentityRole<Guid>>().Add(new IdentityRole<Guid> { Name = ApplicationRoles.Admin, NormalizedName = ApplicationRoles.Admin.ToUpper()});
                    await db.SaveChangesAsync();
                }
                if (await db.Set<IdentityRole<Guid>>().FirstOrDefaultAsync(r => r.Name == ApplicationRoles.User) is null)
                {
                    db.Set<IdentityRole<Guid>>().Add(new IdentityRole<Guid> { Name = ApplicationRoles.User, NormalizedName = ApplicationRoles.User.ToUpper() });
                    await db.SaveChangesAsync();
                }
            });
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>()
            .Property(u => u.FirstName)
            .HasMaxLength(200)
            .IsRequired(false);

        builder.Entity<ApplicationUser>()
            .Property(u => u.LastName)
            .HasMaxLength(200)
            .IsRequired(false);

        builder.Entity<ApplicationUser>()
            .Property(u => u.Country)
            .HasMaxLength(200)
            .IsRequired(false);
    }

}
