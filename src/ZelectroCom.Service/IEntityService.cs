using System.Collections.Generic;
using System.Data;
using ZelectroCom.Data;

namespace ZelectroCom.Service
{
    public interface IEntityService<T> : IService
       where T : Entity
    {
        T Create(T entity);
        void Delete(T entity);
        void Delete(int id);
        T CreateOrUpdate(T entity);
        IEnumerable<T> GetAll();
        T GetById(int id);
        T Update(T entity);
    }
}
