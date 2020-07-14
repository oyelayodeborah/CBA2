using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCBA.Core.Models
{
    public class CustomerAccount
    {
        public int id { get; set; }

        public Customer customer { get; set; }

        [Required(ErrorMessage = ("Customer is required"))]
        [Display(Name = "Customer")]
        public int customerId { get; set; }

        [Required(ErrorMessage = ("Account Name is required")),MaxLength(60,ErrorMessage ="Customer's Account Name cant be more than 60 characters")]
        [RegularExpression(@"^[ a-zA-Z]+$", ErrorMessage = "Account name should  only characters and white spaces")]
        [MinLength(5)]
        [Display(Name = "Account Name")]
        [DataType(DataType.Text)]
        public string acctName { get; set; }

        //[Required(ErrorMessage = ("Account Number is required"))]
        [Display(Name = "Account Number")]
        
        public long acctNumber { get; set; }

        public Branch branch { get; set; }

        [Required(ErrorMessage = ("Branch is required"))]
        [Display(Name = "Branch")]
        public int branchId { get; set; }

        [Required(ErrorMessage = ("Account Type is required"))]
        [Display(Name = "Account Type")]
        public string accType { get; set; }

        //[Required(ErrorMessage = ("Status is required"))]
        [Display(Name = "Status")]
        public string status { get; set; }//if it is closed or opened

        //[Required(ErrorMessage = ("Account Balance is required"))]
        [Display(Name = "Account Balance")]
        public decimal acctbalance { get; set; }

        ////[Required(ErrorMessage = ("Interest is required"))]
        //[Display(Name = "Interest")]
        //public decimal interest { get; set; }

        [Display(Name = "Savings Interest Accrued / Current COT Accrued / Loan Interest Accrued")]
        public decimal dailyInterestAccrued { get; set; }

        //[Required(ErrorMessage = ("Date Created is required"))]
        [Display(Name = "Date Created")]
        public DateTime createdAt { get; set; }

        //[Required(ErrorMessage = ("Is Linked to loan is required"))]
        [Display(Name = "Is Linked to loan")]
        public bool isLinked { get; set; }//if it is linked to a loan or not linked to a loan
    }
}
