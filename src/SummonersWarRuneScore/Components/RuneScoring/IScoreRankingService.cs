using System.Collections.Generic;
using SummonersWarRuneScore.Components.Domain;

namespace SummonersWarRuneScore.Components.RuneScoring
{
	public interface IScoreRankingService
	{
		List<ScoreRankingResult> CalculateRanks(List<RuneScoringResult> scores);
	}
}
