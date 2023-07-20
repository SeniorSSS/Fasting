using Fasting.EFCore.Models;
using Microsoft.EntityFrameworkCore;

namespace Fasting.EFCore;
public class BaseDbContext : DbContext
{
    private readonly IUserAccessor _userAccessor;

    public BaseDbContext() : this(new DbContextOptions<DbContext>(), null)
    { }

    public BaseDbContext(DbContextOptions options) : this(options, null)
    { }

    public BaseDbContext(IUserAccessor userAccessor) : this(new DbContextOptions<DbContext>(), userAccessor)
    { }

    public BaseDbContext(DbContextOptions options, IUserAccessor userAccessor) : base(options)
    {
        _userAccessor = userAccessor;
    }

    public override int SaveChanges()
    {
        OnBeforeSaving();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        OnBeforeSaving();
        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

    }

    private void OnBeforeSaving()
    {
        var entities = ChangeTracker.Entries()
            .Where(x => x.Entity is IBaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

        foreach (var entity in entities)
        {
            var now = DateTime.UtcNow;

            if (entity.State == EntityState.Added)
            {
                ((IBaseEntity)entity.Entity).CreatedDate = now;
            }
            else
            {
                var createdDate = nameof(IBaseEntity.CreatedDate);
                entity.Property(createdDate).IsModified = false;
            }
        }
    }
}
