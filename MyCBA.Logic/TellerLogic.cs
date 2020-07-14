using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCBA.Core.Models;

namespace MyCBA.Logic
{
    public class TellerLogic
    {
        ApplicationDbContext _context = new ApplicationDbContext();
        public bool IsTellerAssigned(int userTeller)
        {
            var assignedteller = _context.Tellers.ToList().Where(c => c.userId == userTeller);
            if (assignedteller != null)
            {
                return true;
            }
            return false;

        }
    }
}
