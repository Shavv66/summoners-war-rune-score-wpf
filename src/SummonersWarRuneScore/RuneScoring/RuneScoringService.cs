using SummonersWarRuneScore.Domain;
using System.Collections.Generic;

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
					if (rune.Set == role.RuneSet)
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
