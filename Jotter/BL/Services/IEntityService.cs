using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Services
{
    public interface IEntityService<T>
    {
        Guid Add(T entity);
        void Delete(T entity);
        void Update(T entity);
        IEnumerable<T> GetAllEntities();
        T GetById(Guid id);
    }
}
