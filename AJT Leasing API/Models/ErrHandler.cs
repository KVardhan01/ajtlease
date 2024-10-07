using System;
using System.IO;
using System.Web;

namespace AJT_Leasing_API.Models
{
    public class ErrHandler
    {
        public static void WriteError(Exception exp, string reqtRefNo, string quoteRefNo, string MethodName)
        {
            try
            {
                string path = "~/Error/" + "Error_" + DateTime.Today.ToString("dd-MM-yy") + ".txt";
                if ((!File.Exists(HttpContext.Current.Server.MapPath(path))))
                {
                    File.Create(HttpContext.Current.Server.MapPath(path)).Close();
                }
                using (StreamWriter w = File.AppendText(HttpContext.Current.Server.MapPath(path)))
                {
                    w.WriteLine("Error Details : ");
                    w.WriteLine("{0}", DateTime.Now.ToString("dd-MM-yy hh:mm:ss"));
                    w.WriteLine("\n");
                    w.WriteLine("Method Name : " + MethodName);
                    w.WriteLine("REFERENCE NO : " + reqtRefNo);
                    w.WriteLine("QUOTE REFERENCE NO : " + quoteRefNo);
                    w.WriteLine("ERROR DESCRIPTION :  " + exp.Message);
                    w.WriteLine("ERROR SOURCE : " + "");
                    w.WriteLine("TARGET SITE : " + exp.TargetSite);
                    w.WriteLine("STACK TRACE : " + exp.StackTrace);
                    w.WriteLine("*******************************************************************************************************************************************************");
                    w.Flush();
                    w.Close();
                }
            }
            catch (Exception ex)
            {
                //WriteError(ex, string.Empty, string.Empty, "WriteError");
            }

        }
        public static void WriteLog(string log_header, string log_desc, string refNo)
        {
            try
            {
                string path = "~/Logs/" + "Log_" + DateTime.Today.ToString("dd-MM-yy") + ".txt";
                if (!File.Exists(HttpContext.Current.Server.MapPath(path)))
                {
                    File.Create(HttpContext.Current.Server.MapPath(path)).Close();
                }
                using (StreamWriter w = File.AppendText(HttpContext.Current.Server.MapPath(path)))
                {
                    w.WriteLine("LOG ENTRY : ");
                    w.WriteLine("{0}", DateTime.Now.ToString("dd-MM-yy hh:mm:ss"));
                    w.WriteLine("\n");
                    w.WriteLine("REFERENCE NO : " + refNo);
                    w.WriteLine("LOG FOR: " + log_header);
                    w.WriteLine("DESCRIPTION :  " + log_desc);
                    w.WriteLine("*******************************************************************************************************************************************************");
                    w.Flush();
                    w.Close();
                }
            }
            catch (Exception ex)
            {
                WriteError(ex, string.Empty, string.Empty, "WriteLog");
            }
        }
    }
}