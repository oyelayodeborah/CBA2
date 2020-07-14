using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCBA.Core.Models;

namespace MyCBA.Logic
{
    public class BranchLogic
    {
        ApplicationDbContext _context = new ApplicationDbContext();

        public bool IsDetailsExist(string name)
        {
            var findDetails = _context.Branches.Where(a => a.name==name).Count();

            if (findDetails == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool IsEditDetailsExist(string name)
        {

            var findDetails = _context.Branches.Where(a => a.name==name).Count();
            if (findDetails > 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


    }
}