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
    public class PaymentService : IGenericService<Payment>
    {
        private readonly BpsAppContext _dbContext;

        public PaymentService(string connectionString)
        {
            _dbContext = new BpsAppContext(connectionString);
        }

        public ICollection<Payment> Get(int pageSize, int pageNumber, Expression<Func<Payment, bool>> predicate = null)
        {
            if (predicate == null)
                return _dbContext.Payments
                    .OrderBy(p => p.PaymentDate)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

            return _dbContext.Payments.Where(predicate).ToList();
        }

        public ICollection<Payment> Get(Expression<Func<Payment, bool>> predicate = null)
        {
            if (predicate == null)
                return _dbContext.Payments
                    .OrderBy(p => p.PaymentDate)
                    .ToList();

            return _dbContext.Payments.Where(predicate).ToList();
        }

        public Payment Get(int id)
        {
            return _dbContext.Payments.FirstOrDefault(p => p.Id == id);
        }

        public void Update(Payment payment)
        {
            _dbContext.Update(payment);
            _dbContext.SaveChanges();
        }

        public void Add(Payment payment)
        {
            _dbContext.Payments.Add(payment);
            _dbContext.SaveChanges();
        }

        public bool Exist(int id)
        {
            return _dbContext.Payments.FirstOrDefault(p => p.Id == id) != null;
        }

        public void Delete(int id)
        {
            _dbContext.Payments.Remove(Get(id));
            _dbContext.SaveChanges();
        }

        public void Delete(Payment payment)
        {
            _dbContext.Payments.Remove(payment);
            _dbContext.SaveChanges();
        }

        public long Count()
        {
            return _dbContext.Payments.LongCount();
        }
    }
}
