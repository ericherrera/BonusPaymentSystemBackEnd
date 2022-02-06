using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BonusPaymentSystem.Service.Interfaces
{
    public interface IGenericService<T>
    {
        public ICollection<T> Get(int pageSize, int pageNumber, Expression<Func<T, bool>> predicate = null);
        public ICollection<T> Get(Expression<Func<T, bool>> predicate = null);
        public T Get(int id);

        public void Update(T item);

        public void Add(T item);
        public bool Exist(int id);
        public void Delete(T item);
        public long Count();
    }
}
