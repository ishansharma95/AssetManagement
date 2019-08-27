using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Interface
{
    public interface Imaintanable<T>
    {
        List<T> GetAll();
        int Create(T obj);
        T Get(int id);
        string Update(T obj);
        void Delete(int id);
    }
}
