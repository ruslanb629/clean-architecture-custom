using Domain.Entities;

namespace Application.Common.Interfaces;

public interface ICachedData
{
    Task<List<Department>> Departments();

    Task<List<Employee>> Employees();

    void Remove(string cacheKey);
}