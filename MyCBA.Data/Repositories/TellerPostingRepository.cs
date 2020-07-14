using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCBA.Core.Models;
using MyCBA.Data.Repositories;

namespace MyCBA.Data.Repositories
{
    public class TellerPostingRepository
    {
        public ApplicationDbContext _context = new ApplicationDbContext();
        public BaseRepository<TellerPosting> tellerPostingRepo = new BaseRepository<TellerPosting>(new ApplicationDbContext());
        //public TellerRepository(ApplicationDbContext context)
        //{
        //    _context = context;
        //}

        //public TellerPosting GetByUser(string user)
        //{
        //    return _context.Tellers.Where(c => c.user == user).FirstOrDefault();
        //}

        //public TellerPosting GetByTillAccount(string tillAccount)
        //{
        //    return _context.Tellers.Where(c => c.tillAccount == tillAccount).FirstOrDefault();
        //}

        public TellerPosting Get(int? id)
        {
            return tellerPostingRepo.Get(id);
        }

        public void Update(TellerPosting tellerPosting)
        {
            tellerPostingRepo.Update(tellerPosting);
        }

        public void Save(TellerPosting tellerPosting)
        {
            tellerPostingRepo.Save(tellerPosting);
        }

    }
}
