using System.Collections;
using PainelIntegraTelefoniaIP.Entity.Interfaces;
using PainelIntegraTelefoniaIP.Models.Interfaces;

namespace PainelIntegraTelefoniaIP.Services.Interfaces
{
    public interface IService<TDto, TEntity>
        where TDto : class, IMeusDtos
        where TEntity : class, IEntity
    {
        void Criar(TDto dto);
        void Editar(int id, TDto dto);
        void Excluir(int id);
        TDto BuscarPorId(int id);
        IEnumerable<TDto> ListarTodos();
        IEnumerable<TDto> ListarComFiltro(IEnumerable filtros);
    }
}