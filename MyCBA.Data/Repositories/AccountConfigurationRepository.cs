using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCBA.Core.Models;

namespace MyCBA.Data.Repositories
{
    public class AccountConfigurationRepository
    {
        BaseRepository<AccountConfiguration> baseRepo = new BaseRepository<AccountConfiguration>(new ApplicationDbContext());

        BaseRepository<SavingsAcctMgt> savRepo = new BaseRepository<SavingsAcctMgt>(new ApplicationDbContext());
        BaseRepository<CurrentAcctMgt> curRepo = new BaseRepository<CurrentAcctMgt>(new ApplicationDbContext());
        BaseRepository<LoanAcctMgt> loanRepo = new BaseRepository<LoanAcctMgt>(new ApplicationDbContext());

        ApplicationDbContext _context = new ApplicationDbContext();

        public AccountConfiguration Get(int? id)
        {
            return baseRepo.Get(id);
        }
        public SavingsAcctMgt GetSavingsId(int? id)
        {
            return savRepo.Get(id);
        }
        public CurrentAcctMgt GetCurrentId(int? id)
        {
            return curRepo.Get(id);
        }
        public LoanAcctMgt GetLoanId(int? id)
        {
            return loanRepo.Get(id);
        }
        public IEnumerable<AccountConfiguration> GetAll()
        {
            return _context.AccountConfigurations.ToList();
        }
        public IEnumerable<SavingsAcctMgt> GetAllSavingsConfig()
        {
            return _context.SavingsAcctMgts.ToList();
        }

        public IEnumerable<CurrentAcctMgt> GetAllCurrentConfig()
        {
            return _context.CurrentAcctMgts.ToList();
        }
        public IEnumerable<LoanAcctMgt> GetAllLoanConfig()
        {
            return _context.LoanAcctMgts.ToList();
        }

        public void Update(AccountConfiguration accountConfiguration)
        {
            baseRepo.Update(accountConfiguration);
        }

        public void Save(AccountConfiguration accountConfiguration)
        {
            baseRepo.Save(accountConfiguration);
        }
        
    }
}
