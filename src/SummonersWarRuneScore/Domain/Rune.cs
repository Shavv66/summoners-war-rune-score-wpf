using Newtonsoft.Json;
using SummonersWarRuneScore.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SummonersWarRuneScore.Domain
{
	public class Rune
	{
		public long Id { get; private set; }
		public long SummonerId { get; private set; }
		public RuneLocation Location { get; private set; }
		public long MonsterId { get; private set; }
		public int Slot { get; private set; }
		public RuneColour Colour { get; private set; }
		public int Stars { get; private set; }
		public RuneSet Set { get; private set; }
		public int MaxLevel { get; private set; }
		public int Level { get; private set; }
		public int SellValue { get; private set; }
		public RuneStat PrimaryStat { get; private set; }
		public RuneStat PrefixStat { get; private set; }
		public List<RuneStat> Substats { get; private set; }
		public RuneQuality Quality { get; private set; }

		//public Rune(int id, int summonerId, RuneLocation location, int monsterId, int slot, RuneColour colour, int stars, RuneSet set, int maxLevel, int level, int sellValue,
		//	RuneStat primaryStat, RuneStat prefixStat, List<RuneStat> substats, RuneQuality quality)
		//{
		//	Id = id;
		//	SummonerId = summonerId;
		//	Location = location;
		//	MonsterId = monsterId;
		//	Slot = slot;
		//	Colour = colour;
		//	Stars = stars;
		//	Set = set;
		//	MaxLevel = maxLevel;
		//	Level = level;
		//	SellValue = sellValue;
		//	PrimaryStat = primaryStat;
		//	PrefixStat = prefixStat;
		//	Substats = substats;
		//	Quality = quality;
		//}

		[JsonConstructor]
		public Rune(long rune_id, long wizard_id, RuneLocation occupied_type, long occupied_id, int slot_no, RuneColour rank, int @class, RuneSet set_id, int upgrade_limit,
			int upgrade_curr, int sell_value, List<int> pri_eff, List<int> prefix_eff, List<List<int>> sec_eff, RuneQuality extra)
		{
			Id = rune_id;
			SummonerId = wizard_id;
			Location = occupied_type;
			MonsterId = occupied_id;
			Slot = slot_no;
			Colour = rank;
			Stars = @class;
			Set = set_id;
			MaxLevel = upgrade_limit;
			Level = upgrade_curr;
			SellValue = sell_value;
			PrimaryStat = new RuneStat(pri_eff);
			PrefixStat = new RuneStat(prefix_eff);
			Substats = sec_eff.Select(stat => new RuneStat(stat)).ToList();
			Quality = extra;
		}
	}
}
