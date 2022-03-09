using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BonusPaymentSystem.Service.Interfaces
{
    public interface IGenericServiceId<T>
    {
        public ICollection<T> Get(int pageSize, int pageNumber, Expression<Func<T, bool>> predicate = null);
        public ICollection<T> Get(Expression<Func<T, bool>> predicate = null);
        public T Get(string name);

        public void Update(T item);

        public void Add(T item);
        public bool Exist(string id);
        public void Delete(T item);
        public long Count();
    }
}
