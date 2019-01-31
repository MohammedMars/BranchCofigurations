using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_First_Task.Log
{
    public class clsLogging
    {
        public static void ErrorLogging(Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}