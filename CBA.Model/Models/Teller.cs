using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCBA.Core.Models
{
    public class Teller
    {
        public int id { get; set; }

        public User user { get; set; }

        [Required(ErrorMessage = ("User is required"))]
        [Display(Name = "User")]
        public int userId { get; set; }

        public GlAccount TillAccount { get; set; }

        [Required(ErrorMessage = ("Till Account is required"))]
        [Display(Name = "Till Account")]
        public int TillAccountId { get; set; }

        public decimal tillAccountBalance { get; set; }

    }
}
