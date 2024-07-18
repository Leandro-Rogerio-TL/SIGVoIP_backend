using System.Collections;
using AutoMapper;
using PainelIntegraTelefoniaIP.Entity.Interfaces;
using PainelIntegraTelefoniaIP.Models.Interfaces;
using PainelIntegraTelefoniaIP.Repository.Interfaces;
using PainelIntegraTelefoniaIP.Services.Interfaces;

namespace PainelIntegraTelefoniaIP.Services;

public class GenericService<TDto, TEntity> : IService<TDto, TEntity>
    where TDto : class, IMeusDtos
    where TEntity : class, IEntity
{
    private readonly IRepository<TEntity> _repository;
    private readonly IMapper _mapper;

    public GenericService(IRepository<TEntity> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public virtual TDto BuscarPorId(int id)
    {
        var entity = _repository.Read(id);
        var dto = _mapper.Map<TDto>(entity);
        return dto;
    }

    public virtual void Criar(TDto dto)
    {
        var entity = _mapper.Map<TEntity>(dto);
        _repository.Create(entity);
    }

    public void Editar(int id, TDto dto)
    {
        var entity = _repository.Read(id);
        if (entity.Equals(null))
            throw new KeyNotFoundException();
        entity = _mapper.Map<TEntity>(dto);
        _repository.Uptade(entity);
    }

    public void Excluir(int id)
    {
        var entity = _repository.Read(id);
        if (entity.Equals(null))
            throw new KeyNotFoundException();
        _repository.Delete(entity);
    }

    public IEnumerable<TDto> ListarComFiltro(IEnumerable filtros)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<TDto> ListarTodos()
    {
        List<TDto> listaDto = null;
        var lista = _repository.List();
        foreach(var entity in lista)
        {
            var dto = _mapper.Map<TDto>(entity);
            if (!dto.Equals(null))
                listaDto.Add(dto);
        }
        return listaDto;
    }
}