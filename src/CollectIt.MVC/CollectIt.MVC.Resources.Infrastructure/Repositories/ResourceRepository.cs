using CollectIt.MVC.Account.Abstractions.Interfaces;
using CollectIt.MVC.Account.Infrastructure.Data;
using CollectIt.MVC.Resources.Abstractions;
using CollectIt.MVC.Resources.Entities;
using Microsoft.EntityFrameworkCore;

namespace CollectIt.MVC.Resources.Infrastructure.Repositories;

public class ResourceRepository : IResourceRepository
{
    private readonly PostgresqlIdentityDbContext context;

    public ResourceRepository(PostgresqlIdentityDbContext context)
    {
        this.context = context;
    }

    public async Task<int> AddAsync(Resource item)
    {
        await context.Resources.AddAsync(item);
        await context.SaveChangesAsync();
        return item.ResourceId;
    }

    public async Task<Resource> FindByIdAsync(int id)
    {
        return await context.Resources
            .Where(resource => resource.ResourceId == id).FirstOrDefaultAsync() ?? throw new InvalidOperationException();
    }

    public Task UpdateAsync(Resource item)
    {
        throw new NotImplementedException();
    }

    public async Task RemoveAsync(Resource item)
    {
        context.Resources.Remove(item);
        await context.SaveChangesAsync();
    }
}