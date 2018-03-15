using System.Collections.Generic;
using SummonersWarRuneScore.Components.Domain;
using SummonersWarRuneScore.Domain;

namespace SummonersWarRuneScore.RuneScoring
{
	public interface IRuneScoringService
	{
		List<RuneScoringResult> CalculateScores(List<Rune> runes, List<MonsterRole> monsterRoles);
	}
}
