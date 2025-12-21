using System.Linq.Expressions;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domain.Repositories;

public abstract class ARepositoryAsync<TEntity> : IRepositoryAsync<TEntity> where TEntity : class
{
    protected readonly DbContext _context;
    private readonly DbSet<TEntity> _table;

    protected ARepositoryAsync(DbContext context)
    {
        _context = context;
        _table = _context.Set<TEntity>();
    }

    public async Task<TEntity> CreateAsync(TEntity t)
    {
        _table.Add(t);
        await _context.SaveChangesAsync();
        return t;
    }

    public async Task<List<TEntity>> CreateRangeAsync(List<TEntity> list)
    {
        await _table.AddRangeAsync(list);
        await _context.SaveChangesAsync();
        return list;
    }

    public async Task UpdateAsync(TEntity t)
    {
        _context.ChangeTracker.Clear();
        _table.Update(t);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateRangeAsync(List<TEntity> list)
    {
        _table.UpdateRange(list);
        await _context.SaveChangesAsync();
    }

    public Task<TEntity?> ReadAsync(int id) => _table.FindAsync(id).AsTask();

    public async Task<List<TEntity>> ReadAsync(Expression<Func<TEntity, bool>> filter) =>
        await _table.Where(filter).ToListAsync();

    public async Task<List<TEntity>> ReadAsync(int start, int count) =>
        await _table.Skip(start).Take(count).ToListAsync();

    public async Task<List<TEntity>> ReadAllAsync() => await _table.ToListAsync();

    public async Task DeleteAsync(TEntity t)
    {
        _table.Remove(t);
        await _context.SaveChangesAsync();
    }
}



