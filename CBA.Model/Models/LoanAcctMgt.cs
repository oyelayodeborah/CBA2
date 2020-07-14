using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCBA.Core.Models
{
    public class LoanAcctMgt
    {
        public IEnumerable<GlAccount> LoanInterestIncome { get; set; }
        public IEnumerable<GlAccount> LoanInterestExpense { get; set; }
        public IEnumerable<GlAccount> LoanInterestReceivable { get; set; }



        public int id { get; set; }

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
        public virtual GlAccount LoanInterestReceivableGL { get; set; }

        public string status { get; set; }

    }
}
