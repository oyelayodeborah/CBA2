using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCBA.Core.Models;
using MyCBA.Data.Repositories;

namespace MyCBA.Data.Repositories
{
    public class TellerRepository
    {
        public ApplicationDbContext _context = new ApplicationDbContext();
        public BaseRepository<Teller> tellerRepo = new BaseRepository<Teller>(new ApplicationDbContext());
        //public TellerRepository(ApplicationDbContext context)
        //{
        //    _context = context;
        //}

        //public Teller GetByUser(string user)
        //{
        //    return _context.Tellers.Where(c => c.user == user).FirstOrDefault();
        //}
        public Teller GetByUser(int user)
        {
            return _context.Tellers.Where(c => c.userId == user).FirstOrDefault();
        }

        //public Teller GetByTillAccount(string tillAccount)
        //{
        //    return _context.Tellers.Where(c => c.tillAccount == tillAccount).FirstOrDefault();
        //}
        public Teller GetByTillAccount(int tillAccount)
        {
            return _context.Tellers.Where(c => c.TillAccountId == tillAccount).FirstOrDefault();
        }

        public Teller Get(int? id)
        {
            return tellerRepo.Get(id);
        }

        public void Update(Teller teller)
        {
            tellerRepo.Update(teller);
        }

        public void Save(Teller teller)
        {
            tellerRepo.Save(teller);
        }

    }
}
