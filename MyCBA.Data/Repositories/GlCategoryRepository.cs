using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCBA.Core.Models;
using MyCBA.Data.Repositories;

namespace MyCBA.Data.Repositories
{
    public class GlCategoryRepository
    {
        private ApplicationDbContext _context = new ApplicationDbContext();
        public BaseRepository<GlCategory> glAccountRepo = new BaseRepository<GlCategory>(new ApplicationDbContext());
        //public GlAccountRepository(ApplicationDbContext context)
        //{
        //    _context = context;
        //}

        public GlCategory GetByName(string name)
        {
            return _context.GlCategories.Where(c => c.name == name).FirstOrDefault();
        }
        public GlCategory GetByContainName(string name)
        {
            return _context.GlCategories.Where(c => c.name .Contains(name)).FirstOrDefault();
        }
        public GlCategory GetByMainCategory(MainAccountCategory mainCategory)
        {
            return _context.GlCategories.Where(c => c.mainAccountCategory == mainCategory).FirstOrDefault();
        }

        //public IEnumerable<GlCategory> GetAllCategoryName(string catName)
        //{
        //   return _context.GlCategories.Where(c => c.GlCategory == catName).ToList();
        //}
        //public GlCategory GetVault()
        //{
        //    return _context.GlCategories.Where(c => c.Name.Contains("vault") && c.mainCategory=="Asset").FirstOrDefault();
        //}
        public GlCategory Get(int? id)
        {
            return glAccountRepo.Get(id);
        }

        public void Update(GlCategory glAccount)
        {
            glAccountRepo.Update(glAccount);
        }

        public void Save(GlCategory glAccount)
        {
            glAccountRepo.Save(glAccount);
        }

    }
}
