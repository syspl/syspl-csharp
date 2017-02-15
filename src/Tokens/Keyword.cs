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

namespace SysPL.Tokens
{
	public enum Keywords
	{
		// Keywords used in Declarations
		Class,
		Deinit,
		Enum,
		Extension,
		Func,
		Import,
		Init,
		Let,
		Interface,
		Static,
		Struct,
		Subscript,
		TypeAlias,
		Var,
		// Keywords used in Statements
		Break,
		Case,
		Continue,
		Default,
		Do,
		Else,
		Fallthrough,
		If,
		In,
		For,
		Return,
		Switch,
		Where,
		While,
		// Keywords used in Expressions and Types
		As,
		DynamicType,
		Is,
		New,
		Super,
		This,
		StaticThis,
		Type,
		Column,
		File,
		Function,
		Line,
		// Keywords reserved in particular contexts
		Associativity,
		DidSet,
		Get,
		Infix,
		InOut,
		Left,
		Mutating,
		None,
		NonMutating,
		Operator,
		Override,
		Postfix,
		Precedence,
		Prefix,
		Right,
		Set,
		WillSet
	}
}
