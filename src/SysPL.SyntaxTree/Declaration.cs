namespace SysPL.SyntaxTree
{
	public abstract class Declaration : Statement {
		readonly Symbol.Expression symbol;
		public Symbol.Expression Symbol { get { return this.symbol; } }
		public Declaration(Symbol.Expression symbol)
		{
			this.symbol = symbol;
		}
	}
}
