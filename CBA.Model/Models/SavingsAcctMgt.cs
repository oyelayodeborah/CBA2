using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCBA.Core.Models
{
    public class SavingsAcctMgt
    {
        public IEnumerable<GlAccount> SavingsInterestPayable { get; set; }
        public IEnumerable<GlAccount> SavingsInterestExpense { get; set; }

        public int id { get; set; }

        [Display(Name = "Savings Credit Interest Rate")]
        [Range(0.00, 100.00)]
        [RegularExpression(@"^[.0-9]+$", ErrorMessage = "Invalid format for interest rate")]
        public decimal SavingsCreditInterestRate { get; set; }

        [Display(Name = "Savings Minimum Balance")]
        [Range(0, (double)decimal.MaxValue)]
        [RegularExpression(@"^[.0-9]+$", ErrorMessage = "Invalid format for minimum balance")]
        public decimal SavingsMinimumBalance { get; set; }

        [Display(Name = "Select Interest Expense GL")]
        public int? SavingsInterestExpenseGLId { get; set; }
        public virtual GlAccount SavingsInterestExpenseGL { get; set; }

        [Display(Name = "Select Interest Payable GL")]
        public int? SavingsInterestPayableGLId { get; set; }
        public virtual GlAccount SavingsInterestPayableGL { get; set; }

        public string status { get; set; }

    }
}
