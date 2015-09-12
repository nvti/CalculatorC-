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

            return new Operand(double.Parse(num));
        }

        static Operator getOperator(string s, ref int index)
        {
            return Operator.CreateOperator(s[index++].ToString());
        }

		static Function getFunction(string s, ref int index)
		{
			string func = "";
			for (; index < s.Length;)
			{
				if (s[index] == '(') break;
				func += s[index++];
			}
			index++;
			Function f = (Function) Operator.CreateOperator(func);
			for(int i = 0; i < f.NumArg; i++)
			{
				if(s[index - 1] != '(' && s[index - 1] != ',') throw new Exception();

				Element e = Evaluate(s, ref index);
				index++;
				f.Inputs.Add(e);
            }

			if (s[index - 1] != ')') throw new Exception();
			//index++;

			return f;
		}
		public static Element Evaluate(string s, ref int i)
        {
            Stack<Element> element = new Stack<Element>();
            Stack<Operator> op = new Stack<Operator>();
            op.Push(new OpenBracket());

            for (; i < s.Length;)
            {
                char c = s[i];
				if (c == ' ')
				{
					i++;
					continue;
				}
                if ((c >= '0' && c <= '9') || c == '.' || c == 'e')
                {
                    element.Push(getDouble(s, ref i));
                    continue;
                }

				if (c == '+' || c == '-' || c == '*' || c == '/' || c == '^')
				{
					Operator temp = getOperator(s, ref i);
					if (temp != null)
					{
						while (temp.Level <= op.Peek().Level && !(temp is OpenBracket))
						{
							Operator o = op.Pop();
							o.MakeInput(element);
							element.Push(o);
						}
						op.Push(temp);
					}
					continue;
				}
                if (c == '(')
                {
                    i++;
                    element.Push(Evaluate(s, ref i));
					continue;
                }
                if (c == ')' || c == ',')
                {
                    while (!(op.Peek() is OpenBracket))
                    {
                        Operator o = op.Pop();
                        o.MakeInput(element);
						element.Push(o);
                    }
                    op.Pop();
                    break;
                }

				element.Push(getFunction(s, ref i));
            }

			if (element.Count != 1) throw new Exception();

			return element.Pop();
        }
    }
}
