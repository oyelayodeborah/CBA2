using MyCBA.Core.Models;
using MyCBA.Data.Repositories;
using MyCBA.Logic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCBAProcessor
{
    public class Processor
    {
        public static List<AtmTerminal> OurTerminals = new AtmTerminalRepository().GetAll().ToList();

        public void BeginProcess()
        {
            UtilityLogic.LogMessage("Initializing nodes...");
     
            ApplicationDbContext _context = new ApplicationDbContext();
            List<Node> nodes = _context.Nodes.ToList();
                                    
            if (nodes == null || nodes.Count() < 1)
            {
                UtilityLogic.LogError("No node is configured!");
            }
            else
            {
                foreach (var node in nodes)
                {
                    try
                    {
                        Console.WriteLine("Trying to start the listener for the port gotten from the db:"+ node.Port);

                        CbaListener.StartUpListener(node.Id.ToString(), node.HostName, Convert.ToInt32(node.Port));
                        UtilityLogic.LogMessage(node.Name + " now listening on port " + node.Port);
                    }
                    catch (Exception ex)
                    {
                        UtilityLogic.LogError("Message: " + ex.Message + " \t InnerException " + ex.InnerException);
                    }
                    

                }
            }            
        }
        public static string InstitutionCode
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["InstitutionCode"];
            }
        }
    }
}
