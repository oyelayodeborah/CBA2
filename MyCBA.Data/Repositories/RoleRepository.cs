using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCBA.Core.Models;

namespace MyCBA.Data.Repositories
{
    public class RoleRepository
    {
        private ApplicationDbContext _context = new ApplicationDbContext();
        public BaseRepository<Role> roleRepo = new BaseRepository<Role>(new ApplicationDbContext());

        public Role Get(int? id)
        {
            return roleRepo.Get(id);
        }
        public IEnumerable<Role> GetByName(string name)
        {
            return _context.Roles.Where(c=>c.name==name);
        }
        public IEnumerable<Role> GetAll()
        {
            return _context.Roles.ToList();
        }
        public void Update(Role role)
        {
            roleRepo.Update(role);
        }

        public void Save(Role role)
        {
            roleRepo.Save(role);
        }
    }
}
