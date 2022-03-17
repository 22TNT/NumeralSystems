using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace NumeralSystems
{
    public class Number
    {
        public int num_base { get; set; }
        public string num_content { get; set; }

        private int CharToInt(char c)
        {
            c = c.ToString().ToUpper()[0];
            if ('0' <= c && c <= '9')
            {
                return c-'0';
            }
            else if ('A' <= c && c <= 'Z')
            {
                return c - 'A' + 10;
            }
            else if (c == '.' || c == ',' || c == '-')
            {
                return c;
            }
            else
            {
                throw new Exception("Invalid character");
            }
        }

        private char IntToChar(int a)
        {
            if (0 > a || 35 < a)
            {
                throw new Exception("Number must be between 0 and 35, was "+a.ToString());
            }
            if (0 <= a && a <= 9)
                return (char)(a  + '0');
            else return (char)(a + 'A' - 10);
        }
        public Number(string content, int base_)
        {
            if (base_ < 1 || base_ >36)
            {
                throw new Exception("Base must be between 1 and 36");
            }
            num_base = base_;
            foreach(char c in content)
            {
                if (CharToInt(c) >= num_base && (c!='.' && c!=',' && c!='-'))
                {
                    throw new Exception("Base doesn't match the number");
                }
            }
            num_content = content;
        }

        public Number(double content)
        {
            num_base = 10;
            num_content = content.ToString().Replace(",", ".");
        }

        public double ConvertToBaseTen()
        {
            if (num_base == 10)
            {
                return Convert.ToDouble(num_content, CultureInfo.InvariantCulture);
            }
            int separator = Math.Max(num_content.IndexOf("."), num_content.IndexOf(","));
            string num_content_whole;
            string num_content_decimal = String.Empty;
            if (separator > 0)
            {
                num_content_whole = num_content.Substring(0, separator);
                num_content_decimal = num_content.Substring(separator + 1);
            }
            else
            {
                num_content_whole = num_content;
            }
            int pos = 1;
            if (num_content_whole.StartsWith("-"))
            {
                num_content_whole = num_content_whole.Substring(1);
                pos = -1;
            }
            int whole;
            double decim = 0;
            whole = CharToInt(num_content_whole[0]);
            for (int i = 1; i < num_content_whole.Length; i++)
            {
                whole *= num_base;
                whole += CharToInt(num_content_whole[i]);
            }
            for (int i = num_content_decimal.Length - 1; i >= 0; i--)
            {
                decim += CharToInt(num_content_decimal[i]);
                decim /= num_base;

            }
            double result = pos * (whole + decim);
            num_content = result.ToString().Replace(",", ".");
            num_base = 10;
            return result;
        }

        public string ConvertToBaseN(int base_, int accuracy = 2)
        {
            if (base_ == num_base)
            {
                return num_content;
            }
            if (base_ == 10)
            {
                return ConvertToBaseTen().ToString().Replace(",", ".");
            }
            ConvertToBaseTen();
            int pos = 1;
            if (num_content.StartsWith("-"))
            {
                pos = -1;
                num_content = num_content.Substring(1);
            }
            string whole = string.Empty;
            int cnt = (int)Math.Truncate(Convert.ToDouble(num_content, CultureInfo.InvariantCulture));
            while (cnt > 0)
            {
                int t = (int)cnt % base_;
                whole += IntToChar(t);
                cnt = cnt / base_;
            }
            char[] arr = whole.ToCharArray();
            Array.Reverse(arr);
            whole = new string(arr);
            string decim = string.Empty;
            double dec = Convert.ToDouble(num_content, CultureInfo.InvariantCulture) 
                - Math.Truncate(Convert.ToDouble(num_content, CultureInfo.InvariantCulture));
            for (int i = 0; i<accuracy; i++)
            {
                dec *= base_;
                int digit = (int)dec;
                dec = dec - (double)digit;
                decim += IntToChar(digit);
            }
            num_base = base_;
            if (pos > 0)
            {
                num_content = whole + "." + decim;
            }
            else
            {
                num_content = "-" + whole + "." + decim;
            }
            return num_content;
        }
    }
}
