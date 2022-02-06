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
    public class CampaingService : IGenericService<Campaing>
    {
        private readonly BpsAppContext _dbContext;

        public CampaingService(string connectionString)
        {
            _dbContext = new BpsAppContext(connectionString);
        }

        public ICollection<Campaing> Get(int pageSize, int pageNumber, Expression<Func<Campaing, bool>> predicate = null)
        {
            if (predicate == null)
                return _dbContext.Campaings
                    .OrderBy(p => p.CreateOn)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

            return _dbContext.Campaings.Where(predicate).ToList();
        }

        public ICollection<Campaing> Get(Expression<Func<Campaing, bool>> predicate = null)
        {
            if (predicate == null)
                return _dbContext.Campaings
                    .OrderBy(p => p.CreateOn)
                    .ToList();

            return _dbContext.Campaings.Where(predicate).ToList();
        }

        public Campaing Get(int id)
        {
            return _dbContext.Campaings.FirstOrDefault(p => p.Id  == id);
        }

        public void Update(Campaing campaing) 
        {
            _dbContext.Update(campaing);
            _dbContext.SaveChanges();
        }

        public void Add(Campaing campaing)
        {
            _dbContext.Campaings.Add(campaing);
            _dbContext.SaveChanges();
        }

        public bool Exist(int id)
        {
            return _dbContext.Campaings.FirstOrDefault(p => p.Id == id) != null;
        }

        public void Delete(int id)
        {
            _dbContext.Campaings.Remove(Get(id));
            _dbContext.SaveChanges();
        }

        public void Delete(Campaing campaing)
        {
            _dbContext.Campaings.Remove(campaing);
            _dbContext.SaveChanges();
        }

        public long Count()
        {
            return _dbContext.Campaings.LongCount();
        }
    }
}
