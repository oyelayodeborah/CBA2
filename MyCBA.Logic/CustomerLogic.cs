using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using MyCBA.Core.Models;

namespace MyCBA.Logic
{
    public class CustomerLogic
    {
        ApplicationDbContext _context = new ApplicationDbContext();
        
        public bool IsDetailsExist(string email, string phonenumber )
        {
            var findDetails = _context.Customers.Where(a => a.email == email || a.phoneNumber == phonenumber).Count();

            if (findDetails==0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool IsEditDetailsExist(string email, string phoneNumber)
        {

            var findDetails = _context.Customers.Where(a => a.email == email || a.phoneNumber == phoneNumber).Count();
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
