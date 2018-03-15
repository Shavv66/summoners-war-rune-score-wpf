using System;
using System.Collections.Generic;
using SummonersWarRuneScore.Components.Domain.Enumerations;

namespace SummonersWarRuneScore.Components.Domain
{
	public class RuneStat : IComparable
	{
		public RuneStatType Type { get; private set; }
		public int Amount { get; private set; }

		public string TypeText => Type.ToDisplayString();
		public string AmountText => $"+{Amount}{(Type.IsPercentStat() ? "%" : "")}";

		public RuneStat(RuneStatType type, int amount)
		{
			Type = type;
			Amount = amount;
		}

		public RuneStat(List<int> statAsIntList) : this((RuneStatType)statAsIntList[0], statAsIntList[1]) { }

		public int CompareTo(object other)
		{
			RuneStat otherStat = other as RuneStat;
			if (Type != otherStat.Type)
			{
				return Type.ToString().CompareTo(otherStat.Type.ToString());
			}

			return Amount.CompareTo(otherStat.Amount);
		}

		public override string ToString()
		{
			if (Type == 0)
			{
				return "";
			}
			return $"{TypeText} {AmountText}";
		}
	}
}
