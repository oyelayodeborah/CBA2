using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;

namespace MyCBA
{
    public class ErrorLogger
    {
        public static void Log(string msg)
        {
            string message = "Error: " + msg + " occured at " + DateTime.Now + "\n";
            Trace.TraceInformation(message);
            Trace.Flush();
        }

        public static void LogMessage(String msg)
        {

            #region LogMessage to File
            System.Diagnostics.Trace.TraceInformation(msg);
            using (StreamWriter logWriter = new StreamWriter(@"C:\Users\OYELAYO\Documents\Appzone\Projects\Switch Project\Logfiles\CBA_MessageLogs.txt", true))
            {

                logWriter.WriteLine(msg + " " + DateTime.Now.ToString("dd/MMM/yyyy hh:mm:ss tt") + Environment.NewLine);
            }
            #endregion

            Console.WriteLine("\n " + msg + " " + DateTime.Now.ToString("dd/MMM/yyyy hh:mm:ss tt") + Environment.NewLine);
        }
        public static void LogError(String msg)
        {

            #region LogMessage to File
            System.Diagnostics.Trace.TraceInformation(msg);
            using (StreamWriter logWriter = new StreamWriter(@"C:\Users\OYELAYO\Documents\Appzone\Projects\Switch Project\Logfiles\CBA_MessageLogs.txt", true))
            {
                logWriter.WriteLine("Error: " + msg + " " + DateTime.Now.ToString("dd/MMM/yyyy hh:mm:ss tt") + Environment.NewLine);
                Console.WriteLine("Error: " + msg + " " + DateTime.Now.ToString("dd/MMM/yyyy hh:mm:ss tt") + Environment.NewLine);
            }
            #endregion

            Console.WriteLine("\n " + msg + " " + DateTime.Now.ToString("dd/MMM/yyyy hh:mm:ss tt") + Environment.NewLine);
        }
    }
}