using SummonersWarRuneScore.Domain;
using System.Collections.Generic;

namespace SummonersWarRuneScore.RuneScoring
{
	public interface IRuneScoringService
	{
		List<RuneScoringResult> CalculateScores(List<Rune> runes, List<MonsterRole> monsterRoles);
	}
}
