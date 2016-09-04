namespace SysPL.SyntaxTree
{
	public abstract class Expression : Statement {
		readonly Type.Expression type;
		public Type.Expression Type { get { return this.type; } }
		public Expression(Type.Expression type)
		{
			this.type = type;
		}
	}
}
