using System;

namespace SummonersWarRuneScore.Domain.Enumerations
{
	public enum RuneStatType
	{
		HpFlat = 1,
		HpPercent = 2,
		AtkFlat = 3,
		AtkPercent = 4,
		DefFlat = 5,
		DefPercent = 6,
		Spd = 8,
		CriRate = 9,
		CriDmg = 10,
		Resistance = 11,
		Accuracy = 12
	}

	public static class RuneStatTypeExtensions
	{
		public static string ToDisplayString(this RuneStatType runeStatType)
		{
			switch (runeStatType)
			{
				case RuneStatType.HpFlat:
				case RuneStatType.HpPercent:
					return "HP";
				case RuneStatType.AtkFlat:
				case RuneStatType.AtkPercent:
					return "ATK";
				case RuneStatType.DefFlat:
				case RuneStatType.DefPercent:
					return "DEF";
				case RuneStatType.Spd: return "SPD";
				case RuneStatType.CriRate: return "CRI Rate";
				case RuneStatType.CriDmg: return "CRI Dmg";
				case RuneStatType.Resistance: return "Resistance";
				case RuneStatType.Accuracy: return "Accuracy";
				default: throw new Exception($"Requested display string for invalid stat type ({runeStatType})");
			}
		}

		public static bool IsPercentStat(this RuneStatType runeStatType)
		{
			if (runeStatType == RuneStatType.HpFlat || runeStatType == RuneStatType.AtkFlat || runeStatType == RuneStatType.DefFlat || runeStatType == RuneStatType.Spd)
			{
				return false;
			}

			return true;
		}
	}
}
