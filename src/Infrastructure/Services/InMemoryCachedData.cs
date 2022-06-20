using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Services
{
    public class InMemoryCachedData : ICachedData
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IApplicationDbContext _dbContext;

        public InMemoryCachedData(IMemoryCache memoryCache, IApplicationDbContext dbContext)
        {
            _memoryCache = memoryCache;
            _dbContext = dbContext;
        }

        public async Task<List<Department>> Departments()
        {
            var result = await
            _memoryCache.GetOrCreateAsync(nameof(Department), async (entry) =>
            {
                entry.SlidingExpiration = TimeSpan.FromHours(24);

                return await _dbContext.Departments.ToListAsync();
            });

            return result;
        }

        public async Task<List<Employee>> Employees()
        {
            var result = await
            _memoryCache.GetOrCreateAsync(nameof(Employee), async (entry) =>
            {
                entry.SlidingExpiration = TimeSpan.FromHours(24);

                return await _dbContext.Employees.ToListAsync();
            });

            return result;
        }

        public void Remove(string cacheKey)
        {
            if (_memoryCache.Get(cacheKey) != null)
                _memoryCache.Remove(cacheKey);
        }
    }
}
