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
    public class ParameterService : IGenericService<Parameter>
    {
        private readonly BpsAppContext _dbContext;

        public ParameterService(string connectionString)
        {
            _dbContext = new BpsAppContext(connectionString);
        }

        public ICollection<Parameter> Get(int pageSize, int pageNumber, Expression<Func<Parameter, bool>> predicate = null)
        {
            if (predicate == null)
                return _dbContext.Parmeters
                    .OrderBy(p => p.CreatedOn)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

            return _dbContext.Parmeters.Where(predicate).ToList();
        }

        public ICollection<Parameter> Get(Expression<Func<Parameter, bool>> predicate = null)
        {
            if (predicate == null)
                return _dbContext.Parmeters
                    .OrderBy(p => p.CreatedOn)
                    .ToList();

            return _dbContext.Parmeters.Where(predicate).ToList();
        }

        public Parameter Get(int id)
        {
            return _dbContext.Parmeters.FirstOrDefault(p => p.Id == id);
        }

        public void Update(Parameter parameter)
        {
            _dbContext.Update(parameter);
            _dbContext.SaveChanges();
        }

        public void Add(Parameter parameter)
        {
            _dbContext.Parmeters.Add(parameter);
            _dbContext.SaveChanges();
        }

        public bool Exist(int id)
        {
            return _dbContext.Parmeters.FirstOrDefault(p => p.Id == id) != null;
        }

        public void Delete(int id)
        {
            _dbContext.Parmeters.Remove(Get(id));
            _dbContext.SaveChanges();
        }

        public void Delete(Parameter parameter)
        {
            _dbContext.Parmeters.Remove(parameter);
            _dbContext.SaveChanges();
        }

        public long Count()
        {
            return _dbContext.Campaings.LongCount();
        }
    }
}
