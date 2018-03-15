using System.Collections.Generic;
using Newtonsoft.Json;
using SummonersWarRuneScore.Components.Domain.Enumerations;

namespace SummonersWarRuneScore.Domain
{
	public class MonsterRole
	{
		public int Id { get; private set; }
		public string Name { get; private set; }
		public List<RuneSet> RuneSets { get; set; }
		public decimal HpPercentWeight { get; set; }
		public decimal AtkPercentWeight { get; set; }
		public decimal DefPercentWeight { get; set; }
		public int ExpectedBaseHp { get; set; }
		public int ExpectedBaseAtk { get; set; }
		public int ExpectedBaseDef { get; set; }
		public decimal SpdWeight { get; set; }
		public decimal CritRateWeight { get; set; }
		public decimal CritDmgWeight { get; set; }
		public decimal ResistanceWeight { get; set; }
		public decimal AccuracyWeight { get; set; }

		public MonsterRole(string name, List<RuneSet> runeSets)
			: this(-1, name, runeSets, 1, 1, 1, 10000, 650, 550, 1.5m, 1.5m, 1m, 0.5m, 1m)
		{
		}

		public MonsterRole(string name, List<RuneSet> runeSets, decimal hpPercentWeight, decimal atkPercentWeight, decimal defPercentWeight, int expectedBaseHp,
			int expectedBaseAtk, int expectedBaseDef, decimal spdWeight, decimal critRateWeight, decimal critDmgWeight, decimal resistanceWeight, decimal accuracyWeight)
			: this(-1, name, runeSets, hpPercentWeight, atkPercentWeight, defPercentWeight, expectedBaseHp, expectedBaseAtk, expectedBaseDef, spdWeight, critRateWeight, critDmgWeight, resistanceWeight, accuracyWeight)
		{
		}

		[JsonConstructor]
		public MonsterRole(int id, string name, List<RuneSet> runeSets, decimal hpPercentWeight, decimal atkPercentWeight, decimal defPercentWeight, int expectedBaseHp,
			int expectedBaseAtk, int expectedBaseDef, decimal spdWeight, decimal critRateWeight, decimal critDmgWeight, decimal resistanceWeight, decimal accuracyWeight)
		{
			Id = id;
			Name = name;
			RuneSets = runeSets;
			HpPercentWeight = hpPercentWeight;
			AtkPercentWeight = atkPercentWeight;
			DefPercentWeight = defPercentWeight;
			ExpectedBaseHp = expectedBaseHp;
			ExpectedBaseAtk = expectedBaseAtk;
			ExpectedBaseDef = expectedBaseDef;
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

		public decimal GetWeight(RuneStatType statType)
		{
			switch(statType)
			{
				case RuneStatType.HpPercent: return HpPercentWeight;
				case RuneStatType.HpFlat: return ExpectedBaseHp == 0 ? 0 : (HpPercentWeight / ExpectedBaseHp) * 100;
				case RuneStatType.AtkPercent: return AtkPercentWeight;
				case RuneStatType.AtkFlat: return ExpectedBaseAtk == 0 ? 0 : (AtkPercentWeight / ExpectedBaseAtk) * 100;
				case RuneStatType.DefPercent: return DefPercentWeight;
				case RuneStatType.DefFlat: return ExpectedBaseDef == 0 ? 0 : (DefPercentWeight / ExpectedBaseDef) * 100;
				case RuneStatType.Spd: return SpdWeight;
				case RuneStatType.CriRate: return CritRateWeight;
				case RuneStatType.CriDmg: return CritDmgWeight;
				case RuneStatType.Resistance: return ResistanceWeight;
				case RuneStatType.Accuracy: return AccuracyWeight;
				default: return 0;
			}
		}

		public void CopyWeightsFrom(MonsterRole otherRole)
		{
			HpPercentWeight = otherRole.HpPercentWeight;
			AtkPercentWeight = otherRole.AtkPercentWeight;
			DefPercentWeight = otherRole.DefPercentWeight;
			ExpectedBaseHp = otherRole.ExpectedBaseHp;
			ExpectedBaseAtk = otherRole.ExpectedBaseAtk;
			ExpectedBaseDef = otherRole.ExpectedBaseDef;
			SpdWeight = otherRole.SpdWeight;
			CritRateWeight = otherRole.CritRateWeight;
			CritDmgWeight = otherRole.CritDmgWeight;
			ResistanceWeight = otherRole.ResistanceWeight;
			AccuracyWeight = otherRole.AccuracyWeight;
		}

		public override string ToString()
		{
			return Name;
		}
	}
}
