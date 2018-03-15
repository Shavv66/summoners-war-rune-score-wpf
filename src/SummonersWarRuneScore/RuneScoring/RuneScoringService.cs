using System.Collections.Generic;
using SummonersWarRuneScore.Components.Domain;
using SummonersWarRuneScore.Domain;

namespace SummonersWarRuneScore.RuneScoring
{
	public class RuneScoringService : IRuneScoringService
	{
		public List<RuneScoringResult> CalculateScores(List<Rune> runes, List<MonsterRole> monsterRoles)
		{
			List<RuneScoringResult> runeScores = new List<RuneScoringResult>();

			foreach (Rune rune in runes)
			{
				foreach (MonsterRole role in monsterRoles)
				{
					if (role.RuneSets.Contains(rune.Set))
					{
						runeScores.Add(new RuneScoringResult(role.Id, rune.Id, CalculateCurrentScore(rune, role)));
					}
				}
			}

			return runeScores;
		}

		private decimal CalculateCurrentScore(Rune rune, MonsterRole role)
		{
			decimal score = 0;
			score += rune.PrimaryStat.Amount * role.GetWeight(rune.PrimaryStat.Type);
			if (rune.PrefixStat != null)
			{
				score += rune.PrefixStat.Amount * role.GetWeight(rune.PrefixStat.Type);
			}
			foreach (RuneStat stat in rune.Substats)
			{
				score += stat.Amount * role.GetWeight(stat.Type);
			}
			return score;
		}
	}
}
