using System.Text;
using System;

namespace ZBMSLibrary.Util
{
    public class Generator
    {
        public static string GenerateAccountNumber()
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();

            for (int i = 0; i < 16; i++)
            {
                int digit = random.Next(0, 10);
                builder.Append(digit);
                if (i == 3 || i == 7 || i == 11) { builder.Append(" "); }
            }

            return builder.ToString();
        }
    }
}