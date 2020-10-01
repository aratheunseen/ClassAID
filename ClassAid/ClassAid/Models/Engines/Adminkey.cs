using System;
using System.Collections.Generic;
using System.Text;

namespace ClassAid.Models.Engines
{
    class Adminkey
    {
        public static string GetCode()
        {
            Random random = new Random();
            string res = "";
            for (int i = 0; i < 6; i++)
            {
                res += (char)random.Next(65, 91);
            }
            return res;
        }
    }
}
