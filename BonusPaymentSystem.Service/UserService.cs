using BonusPaymentSystem.Core.Model;
using BonusPaymentSystem.Core.Data;
using BonusPaymentSystem.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BonusPaymentSystem.Service
{
    public class UserService : IUserService
    {
        private readonly BpsAppContext _dbContext;

        public UserService(string connectionString)
        {
            _dbContext = new BpsAppContext(connectionString);
        }

        public void Add(User client)
        {
            _dbContext.Users.Add(client);
            _dbContext.SaveChanges();
        }

        public void AddNotStorage(User client)
        {
            _dbContext.Users.Add(client);
        }

        public int ApplyChanges()
        {
            return _dbContext.SaveChanges();
        }

        public long Count()
        {
            return _dbContext.Users.LongCount();
        }

        public void Delete(string userName)
        {
            _dbContext.Users.Remove(Get(userName));
            _dbContext.SaveChanges();
        }

        public bool Exist(string userName)
        {
            return _dbContext.Users.FirstOrDefault(p => !string.IsNullOrEmpty(p.UserName)  && p.UserName == userName) != null;
        }

        public ICollection<User> Get(int pageSize, int pageNumber, Expression<Func<User, bool>> predicate = null)
        {
            if (predicate == null)
                return _dbContext.Users
                    .OrderBy(p => p.UserName)
                    .Skip((pageNumber-1) * pageSize)
                    .Take(pageSize)
                    .ToList();

            return _dbContext.Users.Where(predicate).ToList();
        }

        public ICollection<User> Get(Expression<Func<User, bool>> predicate = null)
        {
            if (predicate == null)
                return _dbContext.Users
                    .OrderBy(p => p.UserName)
                    .ToList();

            return _dbContext.Users.Where(predicate).ToList();
        }

        public  User Get(string userName)
        {
            return _dbContext.Users.FirstOrDefault(p => p.UserName == userName);
        }

        public void Update(User client)
        {
            _dbContext.Update(client);
            _dbContext.SaveChanges();
        }
    }
}
