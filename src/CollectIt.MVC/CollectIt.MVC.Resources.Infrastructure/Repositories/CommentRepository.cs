using CollectIt.MVC.Account.Infrastructure.Data;
using CollectIt.MVC.Resources.Abstractions;
using CollectIt.MVC.Resources.Entities;
using Microsoft.EntityFrameworkCore;

namespace CollectIt.MVC.Resources.Infrastructure.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly PostgresqlIdentityDbContext context;

    public CommentRepository(PostgresqlIdentityDbContext context)
    {
        this.context = context;
    }
    
    public async Task<int> AddAsync(Comment item)
    {
        await context.Comments.AddAsync(item);
        await context.SaveChangesAsync();
        return item.CommentId;
    }

    public async Task<Comment> FindByIdAsync(int id)
    {
        return await context.Comments.Where(com => com.CommentId == id).FirstOrDefaultAsync();
    }

    public Task UpdateAsync(Comment item)
    {
        throw new NotImplementedException();
    }

    public async Task RemoveAsync(Comment item)
    {
        context.Comments.Remove(item);
        await context.SaveChangesAsync();
    }
}