using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerManagementSystem.HelperMethods
{
    public class OtpGenerator
    {
        public static string NewOTP()
        {
            return GenerateRndNumber(6);
        }
        public static string GenerateRndNumber(int cnt)
        {
            string[] key2 = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            Random rand1 = new Random();
            string txt = "";
            for (int j = 0; j < cnt; j++)
                txt += key2[rand1.Next(0, 9)];
            return txt;
        }
    }
}
