using CollectIt.MVC.Account.Infrastructure.Data;
using CollectIt.MVC.Resources.Abstractions;
using CollectIt.MVC.Resources.Entities;
using Microsoft.EntityFrameworkCore;

namespace CollectIt.MVC.Resources.Infrastructure.Repositories;

public class VideoRepository : IVideoRepository
{
    private readonly PostgresqlIdentityDbContext context;
    private readonly ResourceRepository resourceRepository;

    public VideoRepository(PostgresqlIdentityDbContext context)
    {
        this.context = context;
        resourceRepository = new ResourceRepository(context);
    }

    public async Task<int> AddAsync(Video item, Resource resource)
    {
        await context.Videos.AddAsync(item);
        await context.SaveChangesAsync();
        await resourceRepository.AddAsync(resource);
        return item.VideoId;
    }

    public async Task<Video> FindByIdAsync(int id)
    {
        return await context.Videos.Where(video => video.VideoId == id).FirstOrDefaultAsync();
    }

    public Task UpdateAsync(Video item)
    {
        throw new NotImplementedException();
    }

    public async Task RemoveAsync(Video item, Resource resource)
    {
        context.Videos.Remove(item);
        await context.SaveChangesAsync();
        await resourceRepository.RemoveAsync(resource);
    }
}