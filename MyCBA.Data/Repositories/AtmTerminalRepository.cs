using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCBA.Core.Models;

namespace MyCBA.Data.Repositories
{
    public class AtmTerminalRepository
    {
        public BaseRepository<AtmTerminal> baseRepo = new BaseRepository<AtmTerminal>(new ApplicationDbContext());

        public IEnumerable<AtmTerminal> GetAll()
        {
            //ApplicationDbContext _context = new ApplicationDbContext();

            //var getList = _context.AtmTerminals.ToList();
            var getList = baseRepo.GetAll();
            return getList;
        }
        public bool isUniqueName(string name)
        {
            bool flag = true;
            if (baseRepo.GetAll().Any(n => n.Name.ToLower().Equals(name.ToLower())))
            {
                flag = false;
            }
            return flag;
        }
        public bool isUniqueName(string oldName, string newName)
        {
            bool flag = true;
            if (!oldName.ToLower().Equals(newName.ToLower()))
            {
                if (baseRepo.GetAll().Any(n => n.Name.ToLower().Equals(newName.ToLower())))
                {
                    flag = false;
                }
            }
            return flag;
        }
        public bool isUniqueCode(string code)
        {
            bool flag = true;
            if (baseRepo.GetAll().Any(n => n.Code.ToLower().Equals(code.ToLower())))
            {
                flag = false;
            }
            return flag;
        }
        public bool isUniqueCode(string oldCode, string newCode)
        {
            bool flag = true;
            if (!oldCode.ToLower().Equals(newCode.ToLower()))
            {
                if (baseRepo.GetAll().Any(n => n.Code.ToLower().Equals(newCode.ToLower())))
                {
                    flag = false;
                }
            }
            return flag;
        }

        public bool isValidTerminal(string terminalCode)
        {
            return baseRepo.GetAll().Any(t => t.Code == terminalCode);
        }

        public AtmTerminal GetName(string Name)
        {
            var value = baseRepo.GetAll().Where(c=>c.Name == Name).FirstOrDefault();
            return value;
        }

    }
}
