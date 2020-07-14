using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCBA.Core.Models
{
    public class AccountConfiguration
    {
        public int id { get; set; }

        [Display(Name = "Business Status")]
        public bool IsBusinessOpen { get; set; }

        public DateTime FinancialDate { get; set; }

        [Display(Name = "Savings Credit Interest Rate")]
        //[Range(0.00, 100.00)]
        //[RegularExpression(@"^[.0-9]+$", ErrorMessage = "Invalid format for interest rate")]
        public decimal SavingsCreditInterestRate { get; set; }

        [Display(Name = "Savings Minimum Balance")]
        //[Range(0, (double)decimal.MaxValue)]
        //[RegularExpression(@"^[.0-9]+$", ErrorMessage = "Invalid format for minimum balance")]
        public decimal SavingsMinimumBalance { get; set; }

        [Display(Name = "Select Interest Expense GL")]
        public int? SavingsInterestExpenseGLId { get; set; }
        public virtual GlAccount SavingsInterestExpenseGL { get; set; }

        [Display(Name = "Select Interest Payable GL")]
        public int? SavingsInterestPayableGLId { get; set; }
        public virtual GlAccount SavingsInterestPayableGL { get; set; }     //Liability



        //current account side
        [Display(Name = "Current Credit Interest Rate")]
        [Range(0.00, 100.00)]
        [RegularExpression(@"^[.0-9]+$", ErrorMessage = "Invalid format")]
        public decimal CurrentCreditInterestRate { get; set; }

        [Display(Name = "Current Minimum Balance")]
        [Range(0, (double)decimal.MaxValue)]
        [RegularExpression(@"^[.0-9]+$", ErrorMessage = "Invalid format")]
        public decimal CurrentMinimumBalance { get; set; }

        [Display(Name = "COT")]
        [Range(0.00, 1000.00)]
        [RegularExpression(@"^[.0-9]+$", ErrorMessage = "Invalid format")]
        public decimal CurrentCot { get; set; }        //Commission On Turnover

        [Display(Name = "Select Interest Expense GL")]
        public int? CurrentInterestExpenseGLId { get; set; }
        public virtual GlAccount CurrentInterestExpenseGL { get; set; }

        [Display(Name = "Select Interest Payable GL")]
        public int? CurrentInterestPayableGLId { get; set; }
        public virtual GlAccount CurrentInterestPayableGL { get; set; }

        [Display(Name = "Select COT Income GL")]
        public int? CurrentCotIncomeGLId { get; set; }
        public virtual GlAccount CurrentCotIncomeGL { get; set; }



        //Loan account side
        [Display(Name = "Loan Debit Interest Rate")]
        [Range(0.00, 100.00)]
        [RegularExpression(@"^[.0-9]+$", ErrorMessage = "Invalid format")]
        public decimal LoanDebitInterestRate { get; set; }

        [Display(Name = "Select Interest Income GL")]
        public int? LoanInterestIncomeGLId { get; set; }
        public virtual GlAccount LoanInterestIncomeGL { get; set; }

        [Display(Name = "Select Interest Expense GL")]
        public int? LoanInterestExpenseGLId { get; set; }
        public virtual GlAccount LoanInterestExpenseGL { get; set; }        //Expense: from where the loan is disbursed

        [Display(Name = "Select Interest Receivable GL")]
        public int? LoanInterestReceivableGLId { get; set; }
        public virtual GlAccount LoanInterestReceivableGL { get; set; }     //Asset

        public string status { get; set; }
    }
}
