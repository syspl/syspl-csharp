using Generic = System.Collections.Generic;

namespace SysPL.SyntaxTree.Symbol
{
	public class Tuple : Expression
	{
		readonly Generic.IEnumerable<Expression> elements;
		public Generic.IEnumerable<Expression> Elements { get { return this.elements; } }
		public Tuple(params Expression[] elements) : this((Generic.IEnumerable<Expression>)elements)
		{ }
		public Tuple(Generic.IEnumerable<Expression> elements)
		{
			this.elements = elements;
		}
	}
}
