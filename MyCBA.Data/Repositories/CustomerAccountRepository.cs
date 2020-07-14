using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCBA.Core.Models;
using MyCBA.Data.Repositories;

namespace MyCBA.Data.Repositories
{
    public class CustomerAccountRepository
    {
        public ApplicationDbContext _context = new ApplicationDbContext();
        public BaseRepository<CustomerAccount> custAcctRepo = new BaseRepository<CustomerAccount>(new ApplicationDbContext());
        //public CustomerAccountRepository(ApplicationDbContext context)
        //{
        //    _context = context;
        //}

        public bool AnyAccountOfType(string type)
        {
            //using (ISession session = NHibernateHelper.OpenSession())
            //{
            //    return session.Query<CustomerAccount>().Any(a => a.AccountType == type);
            //}
            return _context.CustomerAccounts.Any(a => a.accType == type);
        }

        public CustomerAccount GetByAcctNum(long custAcctNum)
        {
            return _context.CustomerAccounts.Where(c => c.acctNumber == custAcctNum).FirstOrDefault();
        }
        //public CustomerAccount GetByAcctNumm(long custAcctNum)
        //{
        //    CustomerAccount newCustAcct = new CustomerAccount();

        //    using (SqlConnection conn = new SqlConnection("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\OYELAYO\\Desktop\\MyCBA(19-10-2019\\MyCBA\\App_Data\\aspnet-MyCBA-20190825034244.mdf;Integrated Security=True;"))
        //    {

        //        using (SqlCommand cmd = new SqlCommand("Select * from CustomerAccounts where acctNumber=" + custAcctNum, conn)) 
        //        {
        //            cmd.CommandType = CommandType.Text;
        //            //Prepare SQL command that we want to query

        //            cmd.Connection = conn;

        //            // open database connection.
        //            conn.Open();

        //            //Execute the query 
        //            SqlDataReader sdr = cmd.ExecuteReader();

        //            ////Retrieve data from table and Display result
        //            while (sdr.Read())
        //            {
        //                object[] values = new object[6];
        //                sdr.GetValues(values);
        //                for (int i = 0; i < 6; i++)
        //                {
        //                    if (i == 0)
        //                    {
        //                        newCustAcct.id = Convert.ToInt32(values[i]);

        //                    }
        //                    if (i == 1)
        //                    {
        //                        newCustAcct.customerId = Convert.ToInt32(values[i]);

        //                    }
        //                    if (i == 2)
        //                    {
        //                        newCustAcct.acctName = Convert.ToString(values[i]);

        //                    }
        //                    if (i == 3)
        //                    {
        //                        newCustAcct.acctNumber = Convert.ToInt64(values[i]);

        //                    }
        //                    if (i == 4)
        //                    {
        //                        newCustAcct.branchId = Convert.ToInt32(values[i]);

        //                    }
        //                    if (i == 5)
        //                    {
        //                         newCustAcct.accType = Convert.ToString(values[i]);

        //                    }
        //                    if (i == 6)
        //                    {
        //                        newCustAcct.status = Convert.ToString(values[i]);

        //                    }
        //                    if (i == 7)
        //                    {
        //                        newCustAcct.acctbalance = Convert.ToDecimal(values[i]);

        //                    }
        //                    if (i == 8)
        //                    {
        //                        newCustAcct.dailyInterestAccrued = Convert.ToDecimal(values[i]);

        //                    }
        //                    if (i == 9)
        //                    {
        //                        newCustAcct.createdAt = Convert.ToDateTime(values[i]);

        //                    }
        //                    if (i == 10)
        //                    {
        //                        newCustAcct.isLinked = Convert.ToBoolean(values[i]);

        //                    }
        //                }

        //            }
        //        }

        //    }
        //    //List<CustomerAccount> custAcct = new List<CustomerAccount>();
        //    //custAcct.Add(newCustAcct);
        //}
        public List<CustomerAccount> GetByType(string type)
        {
            return _context.CustomerAccounts.Where(c => c.accType == type).ToList();
        }

        public CustomerAccount Get(int? id)
        {
            return custAcctRepo.Get(id);
        }
        //public CustomerAccount GetByCustomer(string cust)
        //{
        //    return _context.CustomerAccounts.Where(c => c.customer == cust).FirstOrDefault();
        //}
        public CustomerAccount GetByCustomer(int cust)
        {
            return _context.CustomerAccounts.Where(c => c.customerId == cust).FirstOrDefault();
        }
        public CustomerAccount GetOpenedAccount(long acctNum)
        {
            return _context.CustomerAccounts.Where(c => c.status == "Opened" && c.acctNumber==acctNum).FirstOrDefault();
        }
        //public IEnumerable<CustomerAccount> GetAllCustomer(string cust)
        //{
        //    return _context.CustomerAccounts.Where(c => c.customer == cust).ToList();
        //}
        public IEnumerable<CustomerAccount> GetAllCustomer(int cust)
        {
            return _context.CustomerAccounts.Where(c => c.customerId == cust).ToList();
        }

        public IEnumerable<CustomerAccount> GetAll()
        {
            return _context.CustomerAccounts.ToList();
        }

        public void Update(CustomerAccount customerAccount)
        {
            custAcctRepo.Update(customerAccount);
        }

        public void Save(CustomerAccount customerAccount)
        {
            custAcctRepo.Save(customerAccount);
        }

    }
}
