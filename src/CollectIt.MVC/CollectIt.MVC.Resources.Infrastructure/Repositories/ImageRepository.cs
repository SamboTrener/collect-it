using CollectIt.MVC.Account.IdentityEntities;
using CollectIt.MVC.Resources.Abstractions;
using CollectIt.MVC.Resources.Entities;
using Microsoft.EntityFrameworkCore;

namespace CollectIt.MVC.Resources.Infrastructure.Repositories;

public class ImageRepository : IImageRepository
{    
    private readonly PostgresqlResourcesDbContext context;
    private readonly ResourceRepository resourceRepository;

    public ImageRepository(PostgresqlResourcesDbContext context)
    {
        this.context = context;
        resourceRepository = new ResourceRepository(context);
    }

    public async Task<int> AddAsync(Image item, Resource resource)
    {
        await context.Images.AddAsync(item);
        await context.SaveChangesAsync();
        await resourceRepository.AddAsync(resource);
        return item.ImageId;
    }

    public async Task<Image> FindByIdAsync(int id)
    {
        return await context.Images
            .Include(img => img.Resource)
            .ThenInclude(res => res.ResourceOwner)
            .Where(img => img.ImageId == id).FirstOrDefaultAsync() ?? throw new InvalidOperationException();
    }

    public Task UpdateAsync(Image item)
    {
        throw new NotImplementedException();
    }

    public async Task RemoveAsync(Image item, Resource resource)
    {
        context.Images.Remove(item);
        await resourceRepository.RemoveAsync(resource);
        await context.SaveChangesAsync();
    }
}