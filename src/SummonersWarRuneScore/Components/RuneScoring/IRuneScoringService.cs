using System.Collections.Generic;
using SummonersWarRuneScore.Components.Domain;

namespace SummonersWarRuneScore.Components.RuneScoring
{
	public interface IRuneScoringService
	{
		List<RuneScoringResult> CalculateScores(List<Rune> runes, List<MonsterRole> monsterRoles);
	}
}
