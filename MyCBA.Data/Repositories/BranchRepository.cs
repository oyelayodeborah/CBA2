using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCBA.Core.Models;

namespace MyCBA.Data.Repositories
{
    public class BranchRepository
    {
        private ApplicationDbContext _context = new ApplicationDbContext();
        public BaseRepository<Branch> branchRepo = new BaseRepository<Branch>(new ApplicationDbContext());

        public Branch Get(int? id)
        {
            return branchRepo.Get(id);
        }
        public IEnumerable<Branch> GetByName(string name)
        {
            return _context.Branches.Where(c => c.name == name);
        }
        public IEnumerable<Branch> GetAll()
        {
            return _context.Branches.ToList();
        }
        public void Update(Branch branch)
        {
            branchRepo.Update(branch);
        }

        public void Save(Branch branch)
        {
            branchRepo.Save(branch);
        }
    }
}
