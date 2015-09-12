using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    class Parse
    {
		static string StrNumber = "0123456789.e";
		static string StrOperator = "+-*/^";
		static string StrEnd = "),";

        static Operand getDouble(string s, ref int index)
        {
            string num = "";
            char c = s[index];
            while (StrNumber.IndexOf(c) != -1)
            {
                num += c;
                if (++index == s.Length) break;
                c = s[index];
            }

            return new Operand(double.Parse(num));
        }

        static Operator getOperator(string s, ref int index)
        {
            return (Operator) Element.CreateElement(s[index++].ToString());
        }

		static Function getFunction(string s, ref int index, string func)
		{
			Function f = (Function) Element.CreateElement(func);
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

		static Operand getConst(string s, ref int index, string name)
		{
			return (Operand)Element.CreateElement(name);
		}


		public static Element getSpec(string s, ref int index)
		{
			//get name of spec
			Element e;
			string name = "";
			for (; index < s.Length;)
			{
				if (s[index] == '(')
				{
					index++;
					return getFunction(s, ref index, name);
				}
				
				if((StrOperator + StrEnd).IndexOf(s[index]) != -1)
				{
					index++;
					return getConst(s, ref index, name);
				}
				name += s[index++];
			}

			return null;
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
                if (StrNumber.IndexOf(c) != -1)
                {
                    element.Push(getDouble(s, ref i));
                    continue;
                }

				if (StrOperator.IndexOf(c) != -1)
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
                if (StrEnd.IndexOf(c) != -1)
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

				element.Push(getSpec(s, ref i));
            }

			if (element.Count != 1) throw new Exception();

			return element.Pop();
        }
    }
}
