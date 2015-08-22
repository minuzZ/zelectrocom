using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ZelectroCom.Data;

namespace ZelectroCom.Service
{

public abstract class EntityService<T> : IEntityService<T> where T : Entity, new()
{
       protected IContext _context;
       protected IDbSet<T> _dbset;
 
       public EntityService(IContext context)
       {
           _context = context;
           _dbset = _context.Set<T>();
       }
 
       public virtual T Create(T entity)
       {
           if (entity == null)
           {
               throw new ArgumentNullException("entity");
           }
 
           _dbset.Add(entity);
          _context.SaveChanges();
          return entity;
       }
 
 
       public virtual T Update(T entity)
       {
           if (entity == null) throw new ArgumentNullException("entity");
           _context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
           _context.SaveChanges();
           return entity;
       }

       public virtual T CreateOrUpdate(T entity)
       {
           if (entity == null)
           {
               throw new ArgumentNullException("entity");
           }

           return _dbset.Any(x => x.Id == entity.Id) ? Update(entity) : Create(entity);
       }

       public virtual void Delete(T entity)
       {
           if (entity == null) throw new ArgumentNullException("entity");
           _dbset.Remove(entity);
           _context.SaveChanges(); 
       }

       public virtual void Delete(int id)
       {
           if (id < 0) throw new ArgumentNullException("id");
           var stubEntity = new T() {Id = id};
           _dbset.Attach(stubEntity);
           _dbset.Remove(stubEntity);
           _context.SaveChanges();
       }

       public virtual IEnumerable<T> GetAll()
       {         
            return _dbset.AsEnumerable<T>();
       }
       public virtual T GetById(int id)
       {
           return _dbset.Find(id);
       }
   }
}
