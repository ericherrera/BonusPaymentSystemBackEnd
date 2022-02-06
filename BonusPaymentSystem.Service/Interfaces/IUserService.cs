using BonusPaymentSystem.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BonusPaymentSystem.Service.Interfaces
{
    public interface IUserService
    {
        ICollection<User> Get(int pageSize, int pageNumber, Expression<Func<User, bool>> predicate = null);
        ICollection<User> Get(Expression<Func<User, bool>> predicate = null);
        User Get(string userName);

        void Update(User client);

        void Add(User client);
        bool Exist(string userName);
        void Delete(string userName);
        long Count();
        void AddNotStorage(User client);
    }
}
