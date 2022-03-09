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
    public class UserCampaingService : IUserCampaingService
    {
        private readonly BpsAppContext _dbContext;

        public UserCampaingService(string connectionString)
        {
            _dbContext = new BpsAppContext(connectionString);
        }

        public void Add(UserCampaing userCampaing)
        {
            _dbContext.UserCampaings.Add(userCampaing);
            _dbContext.SaveChanges();
        }

        public bool Exist(string userId)
        {
            return _dbContext.UserCampaings.Any(p => p.SallerId == userId);
        }

        public bool Exist(string userId, int campaingId)
        {
            return _dbContext.UserCampaings.Any(p => p.SallerId == userId && p.CampaingId == campaingId);
        }

        public bool Exist(int campaingId)
        {
            return _dbContext.UserCampaings.Any(p => p.CampaingId == campaingId);
        }

        public ICollection<UserCampaing> Get(int pageSize, int pageNumber, Expression<Func<UserCampaing, bool>> predicate = null)
        {
            if (predicate == null)
                return _dbContext.UserCampaings
                    .OrderBy(p => p.CampaingId)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

            return _dbContext.UserCampaings.Where(predicate).ToList();
        }

        public ICollection<UserCampaing> Get(Expression<Func<UserCampaing, bool>> predicate = null)
        {
            if (predicate == null)
                return _dbContext.UserCampaings
                    .OrderBy(p => p.CampaingId)
                    .ToList();

            return _dbContext.UserCampaings.Where(predicate).ToList();
        }

        public UserCampaing Get(string userId, int campaingId)
        {
            return _dbContext.UserCampaings.FirstOrDefault(p => p.SallerId == userId && p.CampaingId == campaingId);
        }
    }
}
