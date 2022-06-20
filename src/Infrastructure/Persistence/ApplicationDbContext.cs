using Application.Common.DTOs;
using Application.Common.Extentions;
using Application.Common.Interfaces;
using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly ICurrentUserService _currentUserService;
    private readonly ILogger _logger;
    private readonly IMemoryCache _memoryCache;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
        ICurrentUserService currentUserService, ILogger<ApplicationDbContext> logger, IMemoryCache memoryCache) : base(options)
    {
        _currentUserService = currentUserService;
        _logger = logger;
        _memoryCache = memoryCache;
    }

    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Department> Departments => Set<Department>();

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = _currentUserService.UserId;
                    entry.Entity.Created = DateTime.Now;
                    break;

                case EntityState.Modified:
                    entry.Entity.LastModifiedBy = _currentUserService.UserId;
                    entry.Entity.LastModified = DateTime.Now;
                    break;
            }

            string entityName = entry.Entity.GetType().Name;

            if (_memoryCache.Get(entityName) != null)
                _memoryCache.Remove(entityName);
        }

        var changes = GetChanges().ToLogJson();

        _logger.LogInformation("Changes: {@Changes}", changes);

        var result = await base.SaveChangesAsync(cancellationToken);

        return result;
    }

    private List<DataChangeDto> GetChanges()
    {
        var result = new List<DataChangeDto>();
        foreach (var change in ChangeTracker.Entries()
                .Where(a => a.State == EntityState.Modified))
        {
            var changes = new List<ChangesDto>();

            foreach (var prop in change.OriginalValues.Properties)
            {
                var originalValue = change.OriginalValues[prop.Name];
                var currentValue = change.CurrentValues[prop.Name];
                if (!object.Equals(currentValue, originalValue))
                {
                    changes.Add(new(prop.Name, originalValue, currentValue));
                }
            }

            result.Add(new(change.Entity.GetType().Name, change.OriginalValues["Id"].ToString(), changes));
        }

        return result;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }
}
