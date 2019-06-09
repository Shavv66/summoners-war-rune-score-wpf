using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SummonersWarRuneScore.Components.Domain.Enumerations;

namespace SummonersWarRuneScore.Components.Domain
{
	public class Rune
	{
		public long Id { get; }
		public long SummonerId { get; }
		public RuneLocation Location { get; }
		public long MonsterId { get; }
		public byte Slot { get; }
		public RuneColour Colour { get; }
		public byte Stars { get; }
		public RuneSet Set { get; }
		public byte MaxLevel { get; }
		public byte Level { get; }
		public int SellValue { get; }
		public RuneStat PrimaryStat { get; }
		public RuneStat PrefixStat { get; }
		public List<RuneStat> Substats { get; }
		public RuneQuality Quality { get; }

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
		public Rune(long rune_id, long wizard_id, RuneLocation occupied_type, long occupied_id, byte slot_no, RuneColour rank, byte @class, RuneSet set_id, byte upgrade_limit, byte upgrade_curr,
			int sell_value, List<short> pri_eff, List<short> prefix_eff, List<List<short>> sec_eff, RuneQuality extra)
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
