using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    public interface Element
    {
        double Value { get; }
    }

    public abstract class Operator : Element
    {
        public static ListOperator list = new ListOperator();

        public List<Element> Inputs { get; set; }

        public double Value
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

        public Operator Clone()
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
            stack.Push(this);
        }

        public static Operator CreateOperator(string key)
        {
            Operator op;
            if (list.TryGetValue(key, out op))
            {
                return op.Clone();
            }
            return null;
        }
    }

    public class Operand : Element
    {
        public Operand(double v)
        {
            Value = v;
        }
        public double Value { get; set; }
    }

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

    public class ListOperator : Dictionary<string, Operator>
    {
        public ListOperator()
        {
            this.Add("+", new Add());
            this.Add("-", new Minus());
            this.Add("*", new Mul());
            this.Add("/", new Div());
            this.Add("(", new OpenBracket());
            this.Add(")", new CloseBracket());
            this.Add("^", new Pow());
        }
    }
}
