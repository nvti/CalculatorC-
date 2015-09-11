using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
	class Parse
	{
        static Operand getDouble(string s, ref int index)
        {
            string num = "";
            char c = s[index];
            while ((c >= '0' && c <= '9') || c == '.' || c == 'e')
            {
                num += c;
                if (++index == s.Length) break;
                c = s[index];
            }

            index--;
            return new Operand(double.Parse(num));
        }

        static Operator getOperator(string s, ref int index)
        {
            return Operator.CreateOperator(s[index].ToString());
        }

        public static void Evaluate(string s, ref int i)
        {
            for(; i < s.Length; i++)
            {
                char c = s[i];
                if((c >= '0' && c <= '9') || c == '.' || c == 'e')
                {
                    Operand a = getDouble(s, ref i);
                    Console.WriteLine(a.Value);
                    continue;
                }

                if(c == '+' || c == '-' || c == '*' || c == '/')
                {
                    Operator a = getOperator(s, ref i);
                    Console.WriteLine(a.Level);
                    continue;
                }
                if(c == '(')
                {
                    i++;
                    Evaluate(s, ref i);
                }
                if (c == ')') break;
            }
        }
    }
}
