using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCBA.Core.Models;

namespace MyCBA.Data.Repositories
{
    public class SinkNodeRepository
    {
        private ApplicationDbContext _context = new ApplicationDbContext();
        public BaseRepository<SinkNode> Repo = new BaseRepository<SinkNode>(new ApplicationDbContext());

        public bool isUniqueName(string name)
        {
            bool flag = true;
            if (Repo.GetAll().Any(n => n.Name.ToLower().Equals(name.ToLower())))
            {
                flag = false;
            }
            return flag;
        }
        public bool isUniqueName(string oldName, string newName)
        {
            bool flag = true;
            if (!oldName.ToLower().Equals(newName.ToLower()))
            {
                if (Repo.GetAll().Any(n => n.Name.ToLower().Equals(newName.ToLower())))
                {
                    flag = false;
                }
            }
            return flag;
        }

        public SinkNode Get(int? id)
        {
            return Repo.Get(id);
        }
        public IEnumerable<SinkNode> GetByStatus(Status Status)
        {
            return _context.SinkNodes.Where(c => c.Status == Status);
        }
        public IEnumerable<SinkNode> GetByName(string Name)
        {
            return _context.SinkNodes.Where(c => c.Name == Name);
        }
        public IEnumerable<SinkNode> GetAll()
        {
            return _context.SinkNodes.ToList();
        }
        public void Update(SinkNode channel)
        {
            Repo.Update(channel);
        }

        public void Save(SinkNode channel)
        {
            Repo.Save(channel);
        }
    }
}


