using SummonersWarRuneScore.Domain;
using System.Collections.Generic;

namespace SummonersWarRuneScore.RuneScoring
{
	public interface IScoreRankingService
	{
		List<ScoreRankingResult> CalculateRanks(List<RuneScoringResult> scores);
	}
}
