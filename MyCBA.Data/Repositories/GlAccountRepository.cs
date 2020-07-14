using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCBA.Core.Models;
using MyCBA.Data.Repositories;

namespace MyCBA.Data.Repositories
{
    public class GlAccountRepository
    {
        public ApplicationDbContext _context = new ApplicationDbContext();
        public BaseRepository<GlAccount> glAccountRepo = new BaseRepository<GlAccount>(new ApplicationDbContext());
        //public GlAccountRepository(ApplicationDbContext context)
        //{
        //    _context = context;
        //}

        public GlAccount GetByName(string name)
        {
            return _context.GlAccounts.Where(c => c.Name == name).FirstOrDefault();
        }
        public GlAccount GetByCode(long code)
        {
            return _context.GlAccounts.Where(c => c.Code == code).FirstOrDefault();
        }
        //public GlAccount GetByCategoryName(string catName)
        //{
        //    return _context.GlAccounts.Where(c => c.GlCategory == catName).FirstOrDefault();
        //}
        public GlAccount GetByCategoryName(int catName)
        {
            return _context.GlAccounts.Where(c => c.GlCategoryId == catName).FirstOrDefault();
        }
        public List<GlAccount> GetByMainCategory(string mainCategory)
        {
            return _context.GlAccounts.Where(c => c.mainCategory == mainCategory).ToList();
        }
        public IEnumerable<GlAccount> GetName(string catName)
        {
           return _context.GlAccounts.Where(c => c.Name == catName).ToList();
        }
        public IEnumerable<GlAccount> GetAllCategoryName(int catName)
        {
            return _context.GlAccounts.Where(c => c.GlCategoryId == catName).ToList();
        }
        public IEnumerable<GlAccount> GetAllCategoryName(string name)
        {
            GlCategoryRepository glCatRepo = new GlCategoryRepository();
            var getCatId = glCatRepo.GetByContainName(name);
            if (getCatId == null) return new List<GlAccount>();
            return _context.GlAccounts.Where(c => c.GlCategoryId==getCatId.id).ToList();
        }
        public IEnumerable<GlAccount> GetAllAsset()
        {
            GlCategoryRepository glCatRepo = new GlCategoryRepository();
            var getCatId = glCatRepo.GetByContainName("Cash");
            if (getCatId == null) return new List<GlAccount>();
            return _context.GlAccounts.Where(c => c.GlCategoryId != getCatId.id && c.mainCategory=="Asset").ToList();
        }
        public GlAccount GetVault()
        {
            return _context.GlAccounts.Where(c => c.Name.Contains("vault") && c.mainCategory=="Asset").FirstOrDefault();
        }
        public GlAccount GetLoanAccount()
        {
            return _context.GlAccounts.Where(c => c.Name.Contains("loan") && c.mainCategory == "Asset").FirstOrDefault();
        }
        public GlAccount Get(int? id)
        {
            return glAccountRepo.Get(id);
        }

        public void Update(GlAccount glAccount)
        {
            glAccountRepo.Update(glAccount);
        }

        public void Save(GlAccount glAccount)
        {
            glAccountRepo.Save(glAccount);
        }

    }
}
