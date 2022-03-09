using BonusPaymentSystem.Core.Constants;
using BonusPaymentSystem.Core.Data;
using BonusPaymentSystem.Core.Model;
using BonusPaymentSystem.Service.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BonusPaymentSystem.Service
{
    public class UserApplicationService : IUserApplicationService
    {
        private readonly BpsAppContext _dbContext;

        public UserApplicationService(string connectionString)
        {
            _dbContext = new BpsAppContext(connectionString);
        }

        public void Add(ApplicationUser client)
        {
            _dbContext.AppUsers.Add(client);
            _dbContext.SaveChanges();
        }

        public void AddNotStorage(ApplicationUser client)
        {
            _dbContext.AppUsers.Add(client);
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
            _dbContext.AppUsers.Remove(Get(userName));
            _dbContext.SaveChanges();
        }

        public bool Exist(string userName)
        {
            return _dbContext.Users.FirstOrDefault(p => !string.IsNullOrEmpty(p.UserName) && p.UserName == userName) != null;
        }

        public ICollection<ApplicationUser> Get(int pageSize, int pageNumber, Expression<Func<ApplicationUser, bool>> predicate = null)
        {
            if (predicate == null)
                return _dbContext.AppUsers
                    .OrderBy(p => p.UserName)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

            return _dbContext.AppUsers.Where(predicate).ToList();
        }

        public ICollection<ApplicationUser> Get(Expression<Func<ApplicationUser, bool>> predicate = null)
        {
            if (predicate == null)
                return _dbContext.AppUsers
                    .OrderBy(p => p.UserName)
                    .ToList();

            return _dbContext.AppUsers.Where(predicate).ToList();
        }

        public ApplicationUser Get(string userName)
        {
            return  _dbContext.AppUsers.FirstOrDefault(p => p.UserName == userName);
        }

        public void Update(ApplicationUser client)
        {
            _dbContext.Update(client);
            _dbContext.SaveChanges();
        }

        public IEnumerable<ApplicationUser> GetAllUserActiveWithRole(string roleName)
        {
            return (from user in _dbContext.AppUsers
                        /*join userRole in _dbContext.RoleUsers
                        on user.Id equals userRole.UserId
                        join role in _dbContext.Rols
                        on userRole.RoleId equals role.Id*/
                    where /*role.Name == roleName &&*/ user.Status == (int)Status.ACTIVE
                    select user).AsEnumerable();
        }

        public IEnumerable<ApplicationUser> GetAllUserWithCampaing(int campaingId)
        {
            return (from user in _dbContext.AppUsers
                    join userCampaing in _dbContext.UserCampaings
                    on user.Id equals userCampaing.SallerId
                    where userCampaing.CampaingId == campaingId
                    select user).AsEnumerable();
        }
    }
}
