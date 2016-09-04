using System;
using Generic = System.Collections.Generic;

namespace SysPL.SyntaxTree.Symmbol
{
	public class Identifier : Expression {
		readonly string name;
		public string Name { get { return this.name; } }
		public Identifier(string name, Type.Expression type = null) :
			base(type) {
			this.name = name;
		}
	}
}
