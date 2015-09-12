using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
	#region Base class
	public abstract class Element
    {
        public virtual double Value { get; set; }

		public static ListSupport listOp = new ListSupport();

		public static Element CreateElement(string key)
		{
			Element op;
			if (listOp.TryGetValue(key, out op))
			{
				return op.Clone();
			}
			return null;
		}

		public abstract Element Clone();
	}

    public abstract class Operator : Element
    {
		public List<Element> Inputs { get; set; }

        public override double Value
        {
            get
            {
                return Calculate();
            }
        }

        public abstract double Calculate();
        public int Level { get; set; }

        public Operator()
        {
            Inputs = new List<Element>();
        }

        public override Element Clone()
        {
            Operator other = (Operator)this.MemberwiseClone();
            other.Inputs = new List<Element>();
            return other;
        }

        public virtual void MakeInput(Stack<Element> stack)
        {
            Inputs.Clear();
            Inputs.Add(stack.Pop());
            Inputs.Add(stack.Pop());
        }
	}

    public class Operand : Element
    {
		public Operand()
		{
			Value = 0;
		}

        public Operand(double v)
        {
            Value = v;
        }
        public override double Value { get; set; }

		public override Element Clone()
		{
			return (Operand)this.MemberwiseClone();
		}
	}

	#endregion


	#region  Normal operator

	public class Add : Operator
    {
        public Add()
            : base()
        {
            Level = 0;
        }
        public override double Calculate()
        {
            return Inputs[0].Value + Inputs[1].Value;
        }
    }

    public class Minus : Operator
    {
        public Minus()
            : base()
        {
            Level = 1;
        }
        public override double Calculate()
        {
            return Inputs[1].Value - Inputs[0].Value;
        }
    }

    public class Mul : Operator
    {
        public Mul()
            : base()
        {
            Level = 2;
        }
        public override double Calculate()
        {
            return Inputs[0].Value * Inputs[1].Value;
        }
    }

    public class Div : Operator
    {
        public Div()
            : base()
        {
            Level = 2;
        }
        public override double Calculate()
        {
            return Inputs[1].Value / Inputs[0].Value;
        }
    }

    public class CloseBracket : Operator
    {
        public CloseBracket()
        {
            Level = -1;
        }
        public override double Calculate()
        {
            return 0;
        }
    }

    public class OpenBracket : Operator
    {
        public OpenBracket()
        {
            Level = -1;
        }
        public override double Calculate()
        {
            return 0;
        }
    }

    public class Pow : Operator
    {
        public Pow() : base()
        {
            Level = 3;
        }

        public override double Calculate()
        {
            return Math.Pow(Inputs[1].Value, Inputs[0].Value);
        }
    }
	#endregion

	#region Function support

	public abstract class Function : Operator
	{
		public int NumArg { set; get; }
		public abstract override double Calculate();
	}

	public class Sin : Function
    {
        public Sin() : base()
        {
			NumArg = 1;
        }
        public override double Calculate()
        {
            return Math.Sin(Inputs[0].Value);
        }
	}

	public class Logarithm : Function
	{
		public Logarithm() : base()
		{
			NumArg = 2;
		}
		public override double Calculate()
		{
			return Math.Log(Inputs[1].Value) / Math.Log(Inputs[0].Value);
		}
	}

	public class Log : Function
	{
		public Log() : base()
		{
			NumArg = 1;
		}
		public override double Calculate()
		{
			return Math.Log(Inputs[0].Value);
		}
	}

	public class Log10 : Function
	{
		public Log10() : base()
		{
			NumArg = 1;
		}
		public override double Calculate()
		{
			return Math.Log10(Inputs[0].Value);
		}
	}
	public class Cos : Function
	{
		public Cos() : base()
		{
			NumArg = 1;
		}
		public override double Calculate()
		{
			return Math.Cos(Inputs[0].Value);
		}
	}

	public class Tan : Function
	{
		public Tan() : base()
		{
			NumArg = 1;
		}
		public override double Calculate()
		{
			return Math.Tan(Inputs[0].Value);
		}
	}

	#endregion


	#region Const

	public class Pi : Operand
	{
		public Pi()
		{
			Value = Math.PI;
		}
	}
	#endregion

	public class ListSupport : Dictionary<string, Element>
    {
        public ListSupport()
        {
			Add("+", new Add());
			Add("-", new Minus());
			Add("*", new Mul());
            Add("/", new Div());
            Add("(", new OpenBracket());
            Add(")", new CloseBracket());
            Add("^", new Pow());

			//function
            Add("sin", new Sin());
			Add("cos", new Cos());
			Add("tan", new Tan());
			Add("logarithm", new Logarithm());
			Add("log", new Log());
			Add("log10", new Log10());

			//const
			Add("pi", new Pi());
		}
    }
}
