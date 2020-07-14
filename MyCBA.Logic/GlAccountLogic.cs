using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCBA.Core.Models;

namespace MyCBA.Logic
{
    public class GlAccountLogic
    {
        ApplicationDbContext _context = new ApplicationDbContext();
        public bool IsDetailsExist(string GlAccountName)
        {
            var findDetails = _context.GlAccounts.Where(a => a.Name.Contains(GlAccountName)).ToList().Count();

            if (findDetails == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool IsExist(GlAccount GlAccountName)
        {
            var findDetails = _context.GlAccounts.Where(a => a.Name.Contains( GlAccountName.Name)).ToList().Count();

            var findDetail = _context.GlAccounts.Where(a => a.Name.Contains(GlAccountName.Name)).FirstOrDefault();

            if (findDetails > 1)
            {
                return true;
            }
            else
            {
                if (findDetail != null) {
                    if (GlAccountName.id != findDetail.id)
                    {
                        return true;
                    }
                    return true;
                }
                
                else
                {
                    return false;
                }
            }
        }

        public long GenerateGlAccountCode(int GlCategory)
        {
            long code = 0;
            var lastGlCategory = _context.GlCategories.Where(a => a.id == GlCategory).OrderByDescending(a => a.id).FirstOrDefault();
            var lastAccount = _context.GlAccounts.Where(a => a.GlCategoryId == GlCategory).OrderByDescending(a => a.id).FirstOrDefault();
            var glCatCode = lastGlCategory.code;
            var toString = "";
            var count = 0;
            if (lastAccount != null)
            {
                code = lastAccount.Code + 1;
                toString = Convert.ToString(code);
                count = toString.Count();
            }
            else
            {
                
                string id= "00000001";
                string value = glCatCode + id;
                code = Convert.ToInt64(value);
                toString = Convert.ToString(code);
                count = toString.Count();
            }
            if (count > 10)
            {
                int startval = 0;
                int lastIndex = toString.IndexOf("0", startval);
                string removeZero = toString.Remove(lastIndex, 1);
                code = Convert.ToInt64(removeZero);
                return code;
            }

            return code;


        }
    }
}
