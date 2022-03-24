using CollectIt.MVC.Account.Infrastructure.Data;
using CollectIt.MVC.Resources.Abstractions;
using CollectIt.MVC.Resources.Entities;
using Microsoft.EntityFrameworkCore;

namespace CollectIt.MVC.Resources.Infrastructure.Repositories;

public class MusicRepository : IMusicRepository
{
    private readonly PostgresqlIdentityDbContext context;
    private readonly ResourceRepository resourceRepository;

    public MusicRepository(PostgresqlIdentityDbContext context)
    {
        this.context = context;
        resourceRepository = new ResourceRepository(context);
    }

    public async Task<int> AddAsync(Music item, Resource resource)
    {
        await context.Musics.AddAsync(item);
        await context.SaveChangesAsync();
        await resourceRepository.AddAsync(resource);
        return item.MusicId;
    }

    public async Task<Music> FindByIdAsync(int id)
    {
        return await context.Musics.Where(music => music.MusicId == id).FirstOrDefaultAsync();
    }

    public Task UpdateAsync(Music item)
    {
        throw new NotImplementedException();
    }

    public async Task RemoveAsync(Music item, Resource resource)
    {
        context.Musics.Remove(item);
        await resourceRepository.RemoveAsync(resource);
        await context.SaveChangesAsync();
    }
}