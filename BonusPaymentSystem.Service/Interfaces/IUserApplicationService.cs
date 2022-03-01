using BonusPaymentSystem.Core.Model;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BonusPaymentSystem.Service.Interfaces
{
    public interface IUserApplicationService 
    {
        ICollection<ApplicationUser> Get(int pageSize, int pageNumber, Expression<Func<ApplicationUser, bool>> predicate = null);
        ICollection<ApplicationUser> Get(Expression<Func<ApplicationUser, bool>> predicate = null);
        ApplicationUser Get(string userName);

        void Update(ApplicationUser client);

        void Add(ApplicationUser client);
        bool Exist(string userName);
        void Delete(string userName);
        long Count();
        void AddNotStorage(ApplicationUser client);
    }
}
