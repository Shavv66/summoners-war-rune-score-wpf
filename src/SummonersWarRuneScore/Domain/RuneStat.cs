using SummonersWarRuneScore.Domain.Enumerations;

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
	}
}
