using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCBA.Data.Repositories;

namespace MyCBA.Logic
{
    public class UtilityLogic
    {
        TellerRepository tellersRepo = new TellerRepository();
        UserRepository userRepo = new UserRepository();
        RoleRepository roleRepo = new RoleRepository();
        CustomerAccountRepository custAcctRepo = new CustomerAccountRepository();
        CustomerRepository custRepo = new CustomerRepository();
        AccountConfigurationRepository acountConfigRepo = new AccountConfigurationRepository();

        public static void LogMessage(String msg)
        {

            #region LogMessage to File
            System.Diagnostics.Trace.TraceInformation(msg);
            using (StreamWriter logWriter = new StreamWriter(@"C:\Users\OYELAYO\Documents\Appzone\Projects\Switch Project\LogFiles\CBA_MessageLogs.txt", true))
            {
                logWriter.WriteLine(msg + " " + DateTime.Now.ToString("dd/MMM/yyyy hh:mm:ss tt") + Environment.NewLine);
                Console.WriteLine(msg + " " + DateTime.Now.ToString("dd/MMM/yyyy hh:mm:ss tt") + Environment.NewLine);
            }
            #endregion
        }

        public static void LogError(String errorMsg)
        {

            #region LogMessage to File
            System.Diagnostics.Trace.TraceInformation(errorMsg);
            using (StreamWriter logWriter = new StreamWriter(@"C:\Users\OYELAYO\Documents\Appzone\Projects\Switch Project\LogFiles\CBA_MessageLogs.txt", true))
            {
                logWriter.WriteLine(errorMsg + " " + DateTime.Now.ToString("dd/MMM/yyyy hh:mm:ss tt") + Environment.NewLine);
                Console.WriteLine(errorMsg + " " + DateTime.Now.ToString("dd/MMM/yyyy hh:mm:ss tt") + Environment.NewLine);
            }
            #endregion
        }



        public int GetTellersWithoutTill()
        {
            Console.WriteLine("I am in GetTellersWithoutTill method .....");
            var getRole = roleRepo.GetByName("Teller").Single();
            var getAllUsers = userRepo.GetAll().Where(c => c.roleId == getRole.id).Count();
            var users = userRepo.GetAll().Where(c => c.roleId == getRole.id && c.IsAssigned == "false");
            
            var NoTill = 0;
            foreach (var user in users)
            {
                var tillAccount = tellersRepo.GetByUser(user.id);
                if (tillAccount == null)
                {
                    NoTill++;
                }

            }
            return NoTill;
        }
        public int GetCustomersWithoutAccount()
        {
            Console.WriteLine("I am in GetCustomersWithoutAccount method .....");

            var getAllCustomers = custRepo.GetAll().Count();
            var customers = custRepo.GetAll();
            var NoAccount = 0;

            foreach (var customer in customers)
            {
                var getCustomer = custAcctRepo.GetAll().Where(c => c.customerId == customer.id);
                if (getCustomer == null)
                {
                    NoAccount++;
                }

            }
            return NoAccount;
        }
        public string GetConfigStatus()
        {
            var getConfig = acountConfigRepo.GetAll().Single();
            var status = getConfig.status;
            return status;
        }
    }
}
