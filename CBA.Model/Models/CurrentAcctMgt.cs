using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCBA.Core.Models;

namespace MyCBA.Core.Models
{
    public class CurrentAcctMgt
    {

        public IEnumerable<GlAccount> CurrentCotIncome { get; set; }
        public IEnumerable<GlAccount> CurrentInterestExpense { get; set; }
        public IEnumerable<GlAccount> CurrentInterestPayable { get; set; }


        public int id { get; set; }


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

        public string status { get; set; }

    }
}
