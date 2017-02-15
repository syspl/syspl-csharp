// Copyright (C) 2014, 2017  Simon Mika <simon@mika.se>
//
// This file is part of SysPL.
//
// SysPL is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// SysPL is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with SysPL.  If not, see <http://www.gnu.org/licenses/>.
//

using Text = Kean.Text;

namespace SysPL.Tokens
{
	public class Identifier : Token
	{
		public string Name { get; }
		public Identifier(string name, Text.Fragment source) :
			base(source)
		{
			this.Name = name;
		}
		public static Token Parse(string data, Text.Fragment source)
		{
			Token result;
			switch (data)
			{
			// Identifier
				default:
					result = new Identifier(data, source);
					break;
			// Keywords used in Declarations
				case "class":
					result = new Keyword(Keywords.Class, source);
					break;
				case "deinit":
					result = new Keyword(Keywords.Deinit, source);
					break;
				case "enum":
					result = new Keyword(Keywords.Enum, source);
					break;
				case "extension":
					result = new Keyword(Keywords.Extension, source);
					break;
				case "func":
					result = new Keyword(Keywords.Func, source);
					break;
				case "import":
					result = new Keyword(Keywords.Import, source);
					break;
				case "init":
					result = new Keyword(Keywords.Init, source);
					break;
				case "let":
					result = new Keyword(Keywords.Let, source);
					break;
				case "interface":
					result = new Keyword(Keywords.Interface, source);
					break;
				case "static":
					result = new Keyword(Keywords.Static, source);
					break;
				case "struct":
					result = new Keyword(Keywords.Struct, source);
					break;
				case "subscript":
					result = new Keyword(Keywords.Subscript, source);
					break;
				case "typealias":
					result = new Keyword(Keywords.TypeAlias, source);
					break;
				case "var":
					result = new Keyword(Keywords.Var, source);
					break;
			// Keywords used in Statements
				case "break":
					result = new Keyword(Keywords.Break, source);
					break;
				case "case":
					result = new Keyword(Keywords.Case, source);
					break;
				case "continue":
					result = new Keyword(Keywords.Continue, source);
					break;
				case "default":
					result = new Keyword(Keywords.Default, source);
					break;
				case "do":
					result = new Keyword(Keywords.Do, source);
					break;
				case "else":
					result = new Keyword(Keywords.Else, source);
					break;
				case "fallthrough":
					result = new Keyword(Keywords.Fallthrough, source);
					break;
				case "if":
					result = new Keyword(Keywords.If, source);
					break;
				case "in":
					result = new Keyword(Keywords.In, source);
					break;
				case "for":
					result = new Keyword(Keywords.For, source);
					break;
				case "return":
					result = new Keyword(Keywords.Return, source);
					break;
				case "switch":
					result = new Keyword(Keywords.Switch, source);
					break;
				case "where":
					result = new Keyword(Keywords.Where, source);
					break;
				case "while":
					result = new Keyword(Keywords.While, source);
					break;
			// Keywords used in Expressions and Types
				case "as":
					result = new Keyword(Keywords.As, source);
					break;
				case "dynamicType":
					result = new Keyword(Keywords.DynamicType, source);
					break;
				case "is":
					result = new Keyword(Keywords.Is, source);
					break;
//				case "new":
//					result = new Keyword(Keywords.New, region);
//					break;
//				case "base":
//					result = new Keyword(Keywords.Super, region);
//					break;
//				case "this":
//					result = new Keyword(Keywords.This, region);
//					break;
//				case "This":
//					result = new Keyword(Keywords.StaticThis, region);
//					break;
				case "type":
					result = new Keyword(Keywords.Type, source);
					break;
				case "__COLUMN__":
					result = new Keyword(Keywords.Column, source);
					break;
				case "__FILE__":
					result = new Keyword(Keywords.File, source);
					break;
				case "__FUNCTION__":
					result = new Keyword(Keywords.Function, source);
					break;
				case "__LINE__":
					result = new Keyword(Keywords.Line, source);
					break;
			// Keywords reserved in particular contexts
				case "associativity":
					result = new Keyword(Keywords.Associativity, source);
					break;
				case "didSet":
					result = new Keyword(Keywords.DidSet, source);
					break;
				case "get":
					result = new Keyword(Keywords.Get, source);
					break;
				case "infix":
					result = new Keyword(Keywords.Infix, source);
					break;
				case "inout":
					result = new Keyword(Keywords.InOut, source);
					break;
				case "left":
					result = new Keyword(Keywords.Left, source);
					break;
				case "mutating":
					result = new Keyword(Keywords.Mutating, source);
					break;
				case "none":
					result = new Keyword(Keywords.None, source);
					break;
				case "nonmutating":
					result = new Keyword(Keywords.NonMutating, source);
					break;
				case "operator":
					result = new Keyword(Keywords.Operator, source);
					break;
				case "override":
					result = new Keyword(Keywords.Override, source);
					break;
				case "postfix":
					result = new Keyword(Keywords.Postfix, source);
					break;
				case "precedence":
					result = new Keyword(Keywords.Precedence, source);
					break;
				case "prefix":
					result = new Keyword(Keywords.Prefix, source);
					break;
				case "right":
					result = new Keyword(Keywords.Right, source);
					break;
				case "set":
					result = new Keyword(Keywords.Set, source);
					break;
				case "willSet":
					result = new Keyword(Keywords.WillSet, source);
					break;
			// Literals
				case "true":
					result = new BooleanLiteral(true, source);
					break;
				case "false":
					result = new BooleanLiteral(false, source);
					break;
				case "null":
					result = new NullLiteral(source);
					break;
			}
			return result;
		}
	}
}
