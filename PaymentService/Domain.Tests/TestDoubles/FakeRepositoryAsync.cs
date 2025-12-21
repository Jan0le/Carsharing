using System.Linq.Expressions;
using Domain.Interfaces;

namespace Domain.Tests.TestDoubles;

internal sealed class FakeRepositoryAsync<TEntity> : IRepositoryAsync<TEntity> where TEntity : class
{
    private readonly List<TEntity> _items = new();
    private readonly Func<TEntity, int>? _getId;
    private readonly Action<TEntity, int>? _setId;
    private int _nextId = 1;

    public FakeRepositoryAsync(Func<TEntity, int>? getId = null, Action<TEntity, int>? setId = null)
    {
        _getId = getId;
        _setId = setId;
    }

    public Task<TEntity> CreateAsync(TEntity t)
    {
        if (_setId is not null && _getId is not null)
        {
            var currentId = _getId(t);
            if (currentId <= 0)
            {
                _setId(t, _nextId++);
            }
        }

        _items.Add(t);
        return Task.FromResult(t);
    }

    public async Task<List<TEntity>> CreateRangeAsync(List<TEntity> list)
    {
        foreach (var item in list)
        {
            await CreateAsync(item);
        }
        return list;
    }

    public Task UpdateAsync(TEntity t) => Task.CompletedTask;

    public Task UpdateRangeAsync(List<TEntity> list) => Task.CompletedTask;

    public Task<TEntity?> ReadAsync(int id)
    {
        if (_getId is null)
        {
            return Task.FromResult<TEntity?>(null);
        }

        return Task.FromResult(_items.SingleOrDefault(x => _getId(x) == id));
    }

    public Task<List<TEntity>> ReadAsync(Expression<Func<TEntity, bool>> filter)
    {
        var compiled = filter.Compile();
        return Task.FromResult(_items.Where(compiled).ToList());
    }

    public Task<List<TEntity>> ReadAsync(int start, int count) =>
        Task.FromResult(_items.Skip(start).Take(count).ToList());

    public Task<List<TEntity>> ReadAllAsync() => Task.FromResult(_items.ToList());

    public Task DeleteAsync(TEntity t)
    {
        _items.Remove(t);
        return Task.CompletedTask;
    }
}


