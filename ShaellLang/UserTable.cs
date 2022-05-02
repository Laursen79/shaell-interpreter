using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime.Atn;

namespace ShaellLang
{
	public class UserTable : BaseTable
	{
		public UserTable MetaTable { get; set; }
		
		public UserTable() : base("usertable")
		{
			SetValue(new NonEnumerableItem(new SString("length")), new NativeFunc(LengthFunc, 0));
			SetValue(new NonEnumerableItem(new SString("insert")), new NativeFunc(InsertFunc, 1));
			SetValue(new NonEnumerableItem(new SString("set_meta_table")), new NativeFunc(SetMetaTableFunc, 1));
		}

		/// <summary>
		/// Returns the value indexed by the given key. If the key is not found, the key is tied to a SNull value.
		/// </summary>
		/// <param name="key">The key to search for.</param>
		/// <returns>The value that is tied to the key</returns>
		public override RefValue GetValue(IValue key)
		{
			RefValue value = base.GetValue(key);
			if (value != null)
				return value;

			value = MetaTable?.GetValue(key);
			if (value != null)
				return value;

			return base.SetNewValue(key, new SNull());
		}

		public IValue SetMetaTableFunc(IEnumerable<IValue> args)
		{
			var argsArr = args.ToArray();
			if (argsArr.Length != 1)
				throw new ArgumentException("Expected 1 argument");
			if (argsArr[0] is not UserTable)
				throw new Exception("Expected UserTable");
			MetaTable = argsArr[0] as UserTable;
			return new SNull();
		}

	}
}