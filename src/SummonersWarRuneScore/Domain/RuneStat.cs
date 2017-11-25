using Newtonsoft.Json;
using SummonersWarRuneScore.Domain.Enumerations;
using System.Collections.Generic;

namespace SummonersWarRuneScore.Domain
{
	public class RuneStat
	{
		public RuneStatType Type { get; private set; }
		public int Amount { get; private set; }

		public RuneStat(RuneStatType type, int amount)
		{
			Type = type;
			Amount = amount;
		}

		public RuneStat(List<int> statAsIntList) : this((RuneStatType)statAsIntList[0], statAsIntList[1]) { }
	}
}
