using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCBA.Core.Models;

namespace MyCBA.Data.Repositories
{
    public class NodeRepository 
    {
        public BaseRepository<Node> baseRepo = new BaseRepository<Node>(new ApplicationDbContext());
        public IEnumerable<Node> GetAll()
        {
            ApplicationDbContext _context = new ApplicationDbContext();
            //var item = _context.Nodes.FirstOrDefault();
            var getList=  _context.Nodes.ToList();
            //var getList= baseRepo.GetAll();
            return getList;

        }
        public bool isUniqueName(string name)
        {
            bool flag = true;
            if (baseRepo.GetAll().Any(n => n.Name.ToLower().Equals(name.ToLower())))
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
                if (baseRepo.GetAll().Any(n => n.Name.ToLower().Equals(newName.ToLower())))
                {
                    flag = false;
                }
            }
            return flag;
        }

    }
}


