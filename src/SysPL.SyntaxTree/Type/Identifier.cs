namespace SysPL.SyntaxTree.Type
{
	public class Identifier : Expression
	{
		readonly string name;
		public string Name { get { return this.name; } }
		public Identifier(string name)
		{
			this.name = name;
		}
	}
}
