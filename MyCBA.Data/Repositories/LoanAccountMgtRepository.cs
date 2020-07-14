using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCBA.Core.Models;

namespace MyCBA.Data.Repositories
{
    public class LoanAccountMgtRepository
    {
        private ApplicationDbContext _context = new ApplicationDbContext();
        public BaseRepository<LoanAcctMgt> savAcctRepo = new BaseRepository<LoanAcctMgt>(new ApplicationDbContext());

        //public LoanAcctMgt GetByInterestRate()
        //{
        //    return _context.LoanAcctMgts.Where(c => c.DebitInterestRate).FirstOrDefault();
        //}

        public IEnumerable<LoanAcctMgt> GetAll()
        {
            return _context.LoanAcctMgts.ToList();
        }
    }
}
