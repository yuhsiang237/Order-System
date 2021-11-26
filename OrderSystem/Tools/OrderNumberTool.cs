using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderSystem.Tools
{
    public static class OrderNumberTool
    {

       public class Type
        {
            public static string Shipment = "S";
            public static string Return = "R";
        }
        /// <summary>
        /// generate ASCII string
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private static string ASCII(int number)
        {
            return (char)number + "";
        }
        /// <summary>
        /// S + 20211122 + AHJZ5
        /// example: S20211122AHJZ5
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GenerateNumber(string type)
        {
            string date = DateTime.Now.ToString("yyyyMMdd");
            string randStr = "";
            Random rnd = new Random();
            for (int i = 0; i < 5; i++)
            {
                randStr += ASCII(rnd.Next(65, 91)); // ASCII 65~90 A~Z
            }
            return type + date + randStr;
        }
    }
}
