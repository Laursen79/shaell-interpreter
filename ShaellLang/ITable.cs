using System.Collections.Generic;

namespace ShaellLang
{
	public interface ITable
	{
		// Gets a key
		RefValue GetValue(IValue key);
		// Get all keys
		public IEnumerable<IValue> GetKeys();
	}
}