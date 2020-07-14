using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCBA.Core.Models;

namespace MyCBA.ViewModels
{
    public class CustomerAccountViewModels
    {
        public enum AccountType
        {
            Current=1,Savings
        }

        public IEnumerable<Branch> Branches { get; set; }
        public IEnumerable<Customer>  Customers { get; set; }

        public CustomerAccount customerAccount { get; set; }

        public int id { get; set; }
        
        [Required(ErrorMessage = ("Account Type is required"))]
        [Display(Name = "Account Type")]
        public AccountType accType { get; set; }

    }
}
