using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCBA.Core.Models;
using MyCBA.Data.Repositories;

namespace MyCBA.Data.Repositories
{
    public class CurrentAccountMgtRepository
    {
        private ApplicationDbContext _context = new ApplicationDbContext();
        public BaseRepository<CurrentAcctMgt> curAcctRepo = new BaseRepository<CurrentAcctMgt>(new ApplicationDbContext());
        //public CurrentAccountRepository(ApplicationDbContext context)
        //{
        //    _context = context;
        //}

        public CurrentAcctMgt GetByCurrentExpense(string curInterestExpense)
        {
           var result= _context.CurrentAcctMgts.Where(c => c.CurrentInterestExpenseGL.Name.Contains(curInterestExpense)).FirstOrDefault();
            return result;
        }

        public void Update(CurrentAcctMgt currentAcctMgt)
        {
            curAcctRepo.Update(currentAcctMgt);
        }

        public void Save(CurrentAcctMgt currentAcctMgt)
        {
            curAcctRepo.Save(currentAcctMgt);
        }

        public IEnumerable<CurrentAcctMgt> GetAll()
        {
            return _context.CurrentAcctMgts.ToList();
        }

    }
}
