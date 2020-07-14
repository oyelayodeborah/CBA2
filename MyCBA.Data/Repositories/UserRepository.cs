using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCBA.Core.Models;
using MyCBA.Data.Repositories;

namespace MyCBA.Data.Repositories
{
    public class UserRepository
    {
        private ApplicationDbContext _context = new ApplicationDbContext();
        public BaseRepository<User> userRepo = new BaseRepository<User>(new ApplicationDbContext());
        //public UserRepository(ApplicationDbContext context)
        //{
        //    _context = context;
        //}

        //public User GetByUser(string user)
        //{
        //    return _context.Tellers.Where(c => c.user == user).FirstOrDefault();
        //}

        //public User GetByTillAccount(string tillAccount)
        //{
        //    return _context.Tellers.Where(c => c.tillAccount == tillAccount).FirstOrDefault();
        //}

        public User Get(int? id)
        {
            return userRepo.Get(id);
        }
        public IEnumerable<User> GetAll()
        {
            return _context.Users.ToList();
        }

        public void Update(User user)
        {
            userRepo.Update(user);
        }

        public void Save(User user)
        {
            userRepo.Save(user);
        }

    }
}
