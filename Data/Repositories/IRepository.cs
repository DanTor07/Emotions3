using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Borrador3Proyecto.Data.Repositories
{
    public interface IRepository<T>
    {
        T FindById(string id);

        List<T> FindAll();

        void Insert(T entity);

        T Update(T entity);

        void Delete(string id);
    }
}
