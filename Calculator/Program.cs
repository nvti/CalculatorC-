using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
	class Program
	{
		//static ListOperator listOp = new ListOperator();
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
                    //Element e = Connect(s + ")");
                    //Console.WriteLine("Ans: " + e.Value);
                    int i = 0;
                    Element e = Parse.Evaluate(s + ")", ref i);
                    Console.WriteLine("Ans: " + e.Value);
                }
				catch 
				{
					Console.WriteLine("Error!");
				}
			} while (true);
		}
	}
}
