using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCBA.Core.Models;

namespace MyCBA.Data.Repositories
{
    public class LoanCustAcctRepository
    {
        private ApplicationDbContext _context = new ApplicationDbContext();
        public BaseRepository<LoanCustAcct> custAcctRepo = new BaseRepository<LoanCustAcct>(new ApplicationDbContext());
        public IEnumerable<LoanCustAcct> GetAll()
        {
            return _context.LoanCustAccts.ToList();
        }
        public IEnumerable<LoanCustAcct> GetAllUnPaid()
        {
            return _context.LoanCustAccts.Where(c=>c.status!="Paid").ToList();
        }
        public LoanCustAcct GetAccount(int servicingAcctId)
        {
            return _context.LoanCustAccts.Where(c => c.ServicingAccountId ==servicingAcctId).FirstOrDefault();
        }
        public LoanCustAcct Get(int? id)
        {
            return custAcctRepo.Get(id);
        }
        public LoanCustAcct GetUnPaid(long acctNum)
        {
            return _context.LoanCustAccts.Where(c => c.AccountNumber == acctNum && c.status=="UnPaid").FirstOrDefault();
        }
        public void Update(LoanCustAcct customerAccount)
        {
            custAcctRepo.Update(customerAccount);
        }
        public void Update(List<LoanCustAcct> customerAccount)
        {
            custAcctRepo.Update(customerAccount.Single());
        }

        public void Save(LoanCustAcct customerAccount)
        {
            custAcctRepo.Save(customerAccount);
        }
    }
}
