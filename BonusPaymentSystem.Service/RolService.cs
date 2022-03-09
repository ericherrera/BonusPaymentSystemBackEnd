using BonusPaymentSystem.Core.Data;
using BonusPaymentSystem.Core.Model;
using BonusPaymentSystem.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BonusPaymentSystem.Service
{
    public class RolService : IGenericServiceId<Rol>
    {
        private readonly BpsAppContext _dbContext;

        public RolService(string connectionString)
        {
            _dbContext = new BpsAppContext(connectionString);
        }

        public void Add(Rol rol)
        {
            _dbContext.Rols.Add(rol);
            _dbContext.SaveChanges();
        }

        public void AddNotStorage(Rol rol)
        {
            _dbContext.Rols.Add(rol);
        }

        public int ApplyChanges()
        {
            return _dbContext.SaveChanges();
        }

        public long Count()
        {
            return _dbContext.Users.LongCount();
        }

        public void Delete(string name)
        {
            _dbContext.Rols.Remove(Get(name));
            _dbContext.SaveChanges();
        }

        public void Delete(Rol item)
        {
            _dbContext.Rols.Remove(item);
            _dbContext.SaveChanges();
        }

        public bool Exist(string id)
        {
            return _dbContext.Users.FirstOrDefault(p => !string.IsNullOrEmpty(p.Id) && p.Id == id) != null;
        }


        public ICollection<Rol> Get(int pageSize, int pageNumber, Expression<Func<Rol, bool>> predicate = null)
        {
            if (predicate == null)
                return _dbContext.Rols
                    .OrderBy(p => p.Name)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

            return _dbContext.Rols.Where(predicate).ToList();
        }

        public ICollection<Rol> Get(Expression<Func<Rol, bool>> predicate = null)
        {
            if (predicate == null)
                return _dbContext.Rols
                    .OrderBy(p => p.Name)
                    .ToList();

            return _dbContext.Rols.Where(predicate).ToList();
        }

        public Rol Get(string name)
        {
            return _dbContext.Rols.FirstOrDefault(p => p.Name == name);
        }

        public Rol Get(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Rol client)
        {
            _dbContext.Update(client);
            _dbContext.SaveChanges();
        }
    }
}
