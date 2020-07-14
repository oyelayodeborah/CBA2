using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCBA.Core.Models;

namespace MyCBA.Logic
{
    public class GlCategoryLogic
    {
        ApplicationDbContext _context = new ApplicationDbContext();
        public bool IsDetailsExist(string name)
        {
            var findDetails = _context.GlCategories.Where(a=> a.name.Contains(name)).ToList().Count();

            if (findDetails == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsExist(GlCategory name)
        {
            var findDetails = _context.GlCategories.Where(a => a.name.Contains(name.name)).ToList().Count();
            var findDetail = _context.GlCategories.Where(a => a.name.Contains(name.name)).FirstOrDefault();

            if (findDetails >1)
            {
                return false;
            }
            else
            {
                if (findDetail != null)
                {
                    if (name.id != findDetail.id)
                    {
                        return false;
                    }
                    return false;
                }
                
                else
                {
                    return true;
                }
            }
        }

        public long GenerateCode(MainAccountCategory mainAccountCategory)
        {
            string categoryVal = "0";
            long code = 0;
            switch (mainAccountCategory.ToString())
            {
                case "Asset":
                    categoryVal = "1";
                    break;
                case "Capital":
                    categoryVal = "3";
                    break;
                case "Expenses":
                    categoryVal = "5";

                    break;
                case "Income":
                    categoryVal = "4";
                    break;
                case "Liability":
                    categoryVal = "2";
                    break;
                default:
                    break;
            }
            
            var lastGlCategory = _context.GlCategories.Where(a => a.mainAccountCategory == mainAccountCategory).OrderByDescending(a => a.id).FirstOrDefault();
            var firstGlCategory = _context.GlCategories.Where(a => a.mainAccountCategory == mainAccountCategory).FirstOrDefault();

            if (lastGlCategory == null)
            {
                string value = categoryVal + "1";
                code = Convert.ToInt64(value);
            }
            else
            {
                var last = lastGlCategory.code.ToString();
                var index = last.Last();
                if (index=='9')
                {
                    string firstval = firstGlCategory.code + "0";
                    code = Convert.ToInt64(firstval);
                }
                else
                {
                    code = lastGlCategory.code + 1;
                }
                
            }
                return code;
        }
    }
}
