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
    public class SaleService : IGenericService<Sale>
    {
        private readonly BpsAppContext _dbContext;

        public SaleService(string connectionString)
        {
            _dbContext = new BpsAppContext(connectionString);
        }


        public ICollection<Sale> Get(int pageSize, int pageNumber, Expression<Func<Sale, bool>> predicate = null)
        {
            if (predicate == null)
                return _dbContext.Sales
                    .OrderBy(p => p.CreatedOn)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

            return _dbContext.Sales.Where(predicate).ToList();
        }

        public ICollection<Sale> Get(Expression<Func<Sale, bool>> predicate = null)
        {
            if (predicate == null)
                return _dbContext.Sales
                    .OrderBy(p => p.CreatedOn)
                    .ToList();

            return _dbContext.Sales.Where(predicate).ToList();
        }
        public Sale Get(int id)
        {
            return _dbContext.Sales.FirstOrDefault(p => p.Id == id);
        }

        public void Update(Sale item)
        {
            _dbContext.Update(item);
            _dbContext.SaveChanges();
        }

        public void Add(Sale sale)
        {
            _dbContext.Sales.Add(sale);
            _dbContext.SaveChanges();
        }

        public bool Exist(int id)
        {
            return _dbContext.Campaings.FirstOrDefault(p => p.Id == id) != null;
        }

        public void Delete(int id)
        {
            _dbContext.Sales.Remove(Get(id));
            _dbContext.SaveChanges();
        }

        public void Delete(Sale sale)
        {
            _dbContext.Sales.Remove(sale);
            _dbContext.SaveChanges();
        }

        public long Count()
        {
            return _dbContext.Campaings.LongCount();
        }

    }
}
