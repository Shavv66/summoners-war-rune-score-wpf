using Newtonsoft.Json;

namespace SummonersWarRuneScore.Domain
{
	public class MonsterRole
	{
		public int Id { get; private set; }
		public string Name { get; private set; }
		public RuneSet RuneSet { get; private set; }
		public decimal HpFlatWeight { get; set; }
		public decimal HpPercentWeight { get; set; }
		public decimal AtkFlatWeight { get; set; }
		public decimal AtkPercentWeight { get; set; }
		public decimal DefFlatWeight { get; set; }
		public decimal DefPercentWeight { get; set; }
		public decimal SpdWeight { get; set; }
		public decimal CritRateWeight { get; set; }
		public decimal CritDmgWeight { get; set; }
		public decimal ResistanceWeight { get; set; }
		public decimal AccuracyWeight { get; set; }

		public MonsterRole(string name, RuneSet runeSet)
			: this(-1, name, runeSet, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
		{
		}

		[JsonConstructor]
		public MonsterRole(int id, string name, RuneSet runeSet, decimal hpFlatWeight, decimal hpPercentWeight, decimal atkFlatWeight, decimal atkPercentWeight, decimal defFlatWeight,
			decimal defPercentWeight, decimal spdWeight, decimal critRateWeight, decimal critDmgWeight, decimal resistanceWeight, decimal accuracyWeight)
		{
			Id = id;
			Name = name;
			RuneSet = runeSet;
			HpFlatWeight = hpFlatWeight;
			HpPercentWeight = hpPercentWeight;
			AtkFlatWeight = atkFlatWeight;
			AtkPercentWeight = atkPercentWeight;
			DefFlatWeight = defFlatWeight;
			DefPercentWeight = defPercentWeight;
			SpdWeight = spdWeight;
			CritRateWeight = critRateWeight;
			CritDmgWeight = critDmgWeight;
			ResistanceWeight = resistanceWeight;
			AccuracyWeight = accuracyWeight;
		}

		public bool IsNew()
		{
			return Id < 0;
		}
	}
}
