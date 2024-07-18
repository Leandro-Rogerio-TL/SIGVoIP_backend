using PainelIntegraTelefoniaIP.Entity.Interfaces;

namespace PainelIntegraTelefoniaIP.Repository.Interfaces;

public interface IRepository<T>
    where T : class, IEntity
{
    void Create(T entity);
    T Read(int id);
    void Uptade(T entity);
    void Delete(T entity);
    IEnumerable<T> List();
}