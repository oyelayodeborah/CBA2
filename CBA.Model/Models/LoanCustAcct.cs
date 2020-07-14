using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCBA.Core.Models
{
    public enum TermsOfLoan
    {
        Fixed=1, Reducing
    }
    public class LoanCustAcct
    {
        //public enum RepaymentPlan
        //{
        //    Annually = 1,
        //    Biannually = 2,
        //    Quarterly = 3,
        //    Monthly = 12
        //}
        public IEnumerable<CustomerAccount> ServicingAccounts { get; set; }

        public int Id { get; set; }

        [Display(Name ="Select Customer Account")]
        public CustomerAccount ServicingAccount { get; set; }

        public int ServicingAccountId { get; set; }

        public long AccountNumber { get; set; }

        [Display(Name ="Loan Amount")]
        public decimal LoanAmount { get; set; }

        public decimal Interest { get; set; }  //LoanAmount*InterestRate/100 

        public decimal InterestRate { get; set; } //from loan acctconfig

        public decimal LoanAmountReduction { get; set; }//loan amount /duration in days //daily principal reduction

        public decimal LoanInterestReduction { get; set; }//loan InterestRate /duration in days //daily interest reduction

        public decimal LoanAmountRemaining { get; set; }//LoanAmount at first  // LoanPrincipalRemaining-LoanPrincipalReduction after

        public decimal LoanInterestRemaining { get; set; } //LoanAmount*Interest/100 at first //LoanInterestRemaining-LoanInterestReduction after
        public decimal LoanMonthlyInterestRepay { get; set; }
        public decimal LoanMonthlyPrincipalRepay { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int DaysCount { get; set; }

        [Display(Name ="Duration In Months")]
        public int DurationInMonths { get; set; }

        //public Duration DurationInMonths { get; set; }

        public string status { get; set; }

        [Display(Name = "Terms Of Loan")]
        public TermsOfLoan? termsOfLoan { get; set; }
        //[Display(Name= "RepaymentPlan")]
        //public RepaymentPlan repaymentPlan { get; set; }

    }
}
