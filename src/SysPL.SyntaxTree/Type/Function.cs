namespace SysPL.SyntaxTree.Type
{
	public class Function : Expression
	{
		readonly string name;
		public string Name { get { return this.name; } }
		readonly Expression argument;
		public Expression Argument { get { return this.argument; } }
		readonly Expression result;
		public Expression Result { get { return this.result; } }
		public Function(string name, Expression argument, Expression result)
		{
			this.name = name;
			this.argument = argument;
			this.result = result;
		}
	}
}
