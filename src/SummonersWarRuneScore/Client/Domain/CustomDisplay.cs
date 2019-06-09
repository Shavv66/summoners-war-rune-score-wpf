using System;

namespace SummonersWarRuneScore.Client.Domain
{
	public class CustomDisplay<T> : IComparable
	{
		private readonly T mValue;
		private readonly Func<T, string> mDisplayFunction;

		public CustomDisplay(T value, Func<T, string> displayFunction)
		{
			mValue = value;
			mDisplayFunction = displayFunction;

		}

		public int CompareTo(object other)
		{
			if (mValue is IComparable value && other is CustomDisplay<T> otherCustomDisplay)
			{
				return value.CompareTo(otherCustomDisplay.mValue);
			}

			return 0;
		}

		public override string ToString()
		{
			return mDisplayFunction(mValue);
		}
	}
}
