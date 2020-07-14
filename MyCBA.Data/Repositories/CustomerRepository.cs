using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCBA.Core.Models;
using MyCBA.Data.Repositories;

namespace MyCBA.Data.Repositories
{
    public class CustomerRepository
    {
        private ApplicationDbContext _context = new ApplicationDbContext();
        public BaseRepository<Customer> custAcctRepo = new BaseRepository<Customer>(new ApplicationDbContext());
        
        public Customer Get(int? id)
        {
            return custAcctRepo.Get(id);
        }
        
        public IEnumerable<Customer> GetAll()
        {
            return _context.Customers.ToList();
        }

        public void Update(Customer customerAccount)
        {
            custAcctRepo.Update(customerAccount);
        }

        public void Save(Customer customerAccount)
        {
            custAcctRepo.Save(customerAccount);
        }

    }
}
