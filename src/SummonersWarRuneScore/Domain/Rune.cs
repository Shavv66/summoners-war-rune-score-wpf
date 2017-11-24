using SummonersWarRuneScore.Domain.Enumerations;
using System.Collections.Generic;

namespace SummonersWarRuneScore.Domain
{
	public class Rune
	{
		public int Id { get; private set; }
		public int SummonerId { get; private set; }
		public RuneLocation Location { get; private set; }
		public int MonsterId { get; private set; }
		public int Slot { get; private set; }
		public RuneColour Colour { get; private set; }
		public int Stars { get; private set; }
		public RuneSet Set { get; private set; }
		public int Level { get; private set; }
		public int SellValue { get; private set; }
		public RuneStat PrimaryStat { get; private set; }
		public RuneStat PrefixStat { get; private set; }
		public List<RuneStat> Substats { get; private set; }
		public RuneQuality Quality { get; private set; }

		public Rune(int id, int summonerId, RuneLocation location, int monsterId, int slot, RuneColour colour, int stars, RuneSet set, int level, int sellValue, RuneStat primaryStat,
			RuneStat prefixStat, List<RuneStat> substats, RuneQuality quality)
		{
			Id = id;
			SummonerId = summonerId;
			Location = location;
			MonsterId = monsterId;
			Slot = slot;
			Colour = colour;
			Stars = stars;
			Set = set;
			Level = level;
			SellValue = sellValue;
			PrimaryStat = primaryStat;
			PrefixStat = prefixStat;
			Substats = substats;
			Quality = quality;
		}

		public Rune(int rune_id)
		{

		}
	}
}
