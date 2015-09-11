using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
	class Program
	{
		static ListOperator listOp = new ListOperator();
		static void Main(string[] args)
		{
			do
			{
				Console.Write(">> ");

				string s = Console.ReadLine();
				if (s == "exit") break;
				if (s == "") continue;
				try
				{
					Element e = Connect(s + ")");
					Console.WriteLine("Ans: " + e.Value);
				}
				catch 
				{
					Console.WriteLine("Error!");
				}
			} while (true);
		}

		static Element Connect(string s)
		{
			Stack<Element> element = new Stack<Element>();
			Stack<Operator> op = new Stack<Operator>();
			op.Push(new OpenBracket());
			string num = "";

			for (int i = 0; i < s.Length; i++)
			{
				char c = s[i];
				if (c == ' ') continue;
				if ((c >= '0' && c <= '9') || c == '.' || c == 'e')
				{
					num += c;
					continue;
				}
				if (num != "")
				{
					element.Push(new Operand(Double.Parse(num)));
					num = "";
				}

				Operator temp = Operator.CreateOperator(c.ToString());
				if (temp != null)
				{
					if (temp is CloseBracket)
					{
						while (!(op.Peek() is OpenBracket))
						{
							Operator o = op.Pop();
							o.MakeInput(element);
						}
						op.Pop();
						continue;
					}
					while (temp.Level <= op.Peek().Level && !(temp is OpenBracket))
					{
						Operator o = op.Pop();
                        o.MakeInput(element);
					}
					op.Push(temp);
				}
			}

			return element.Pop();
		}
	}
}
