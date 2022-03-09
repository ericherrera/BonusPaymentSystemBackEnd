using BonusPaymentSystem.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BonusPaymentSystem.Service.Interfaces
{
    public interface IUserCampaingService
    {
        ICollection<UserCampaing> Get(int pageSize, int pageNumber, Expression<Func<UserCampaing, bool>> predicate = null);
        ICollection<UserCampaing> Get(Expression<Func<UserCampaing, bool>> predicate = null);
        UserCampaing Get(string userId, int campaingId);
        void Add(UserCampaing userCampaing);
        bool Exist(string userId);
        bool Exist(string userId, int campaingId);
        bool Exist(int campaingId);
    }
}
