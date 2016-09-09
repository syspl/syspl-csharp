using Kean.Extension;
using Generic = System.Collections.Generic;

namespace SysPL.SyntaxTree
{
	public class Block : Expression
	{
		readonly Statement[] statements;
		public Generic.IEnumerable<Statement> Statements { get { return this.statements; } }
		public Block(Generic.IEnumerable<Statement> statements) : this(statements.ToArray()) { }
		public Block(params Statement[] statements) : base(Block.GetType(statements.Last()))
		{
			this.statements = statements;
		}
		static Type.Expression GetType(Statement statement)
		{
			return statement is Expression ? (statement as Expression).Type : new Type.Identifier("void");
		}
	}
}
