using Microsoft.EntityFrameworkCore;
using PainelIntegraTelefoniaIP.Entity.Interfaces;
using PainelIntegraTelefoniaIP.Repository.Interfaces;

namespace PainelIntegraTelefoniaIP.Repository;

public class MySqlRepository<T> : IRepository<T>
    where T : class, IEntity
{
    private DbContext _context;
    public MySqlRepository(DbContext context)
    {
        this._context = context;
    }

    public void Create(T entity)
    {
        _context.Add(entity);
    }

    public void Delete(T entity)
    {
        _context.Remove(entity);
    }

    public IEnumerable<T> List()
    {
        return _context.Set<T>().ToList();
    }

    public T Read(int id)
    {
        return _context.Find<T>(id);
    }

    public void Uptade(T entity)
    {
        _context.Update(entity);
    }
}