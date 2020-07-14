using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCBA.Core.Models;
using MyCBA.Data.Repositories;

namespace MyCBA.Data.Repositories
{
    public class SavingsAccountMgtRepository
    {
        private ApplicationDbContext _context = new ApplicationDbContext();
        public BaseRepository<SavingsAcctMgt> savAcctRepo = new BaseRepository<SavingsAcctMgt>(new ApplicationDbContext());
        //public SavingsAccountRepository(ApplicationDbContext context)
        //{
        //    _context = context;
        //}

        public SavingsAcctMgt GetBySavingsExpense(string savInterestExpense)
        {
            return _context.SavingsAcctMgts.Where(c => c.SavingsInterestExpenseGL.Name.Contains(savInterestExpense)).FirstOrDefault();
        }

        public void Update(SavingsAcctMgt savingsAcctMgt)
        {
            savAcctRepo.Update(savingsAcctMgt);
        }

        public void Save(SavingsAcctMgt savingsAcctMgt)
        {
            savAcctRepo.Save(savingsAcctMgt);
        }
        public IEnumerable<SavingsAcctMgt> GetAll()
        {
            return _context.SavingsAcctMgts.ToList();
        }

    }
}
