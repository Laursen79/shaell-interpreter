using System.Collections.Generic;
using System.Collections;

namespace ShaellLang
{
	public interface IValue
	{
		bool ToBool();

		Number ToNumber();

		IFunction ToFunction();

		SString ToSString();

		ITable ToTable();

		SProcess ToSProcess();

		bool IsEqual(IValue other);

		IValue Unpack();
	}
}