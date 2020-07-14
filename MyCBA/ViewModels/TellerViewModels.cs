using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCBA.Core.Models;

namespace MyCBA.ViewModels
{
    public class TellerViewModels
    {
        public IEnumerable<User> Tellers { get; set; }

        public IEnumerable<GlAccount> TillAccounts { get; set; }

        public User user { get; set; }
        public GlAccount TillAccount { get; set; }

        public Teller Teller;

        public int id { get; set; }

        [Required(ErrorMessage = ("User is required"))]
        [Display(Name = "User")]
        public int userId { get; set; }

        [Required(ErrorMessage = ("Till Account is required"))]
        [Display(Name = "Till Account")]
        public int TillAccountId { get; set; }
    }
}
