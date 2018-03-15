using System.Collections.Generic;
using SummonersWarRuneScore.Components.Domain;
using SummonersWarRuneScore.Components.Domain.Enumerations;

namespace SummonersWarRuneScore.RuneScoring
{
	public interface IScoreRankCache
	{
		ScoreRankingResult GetRank(int roleId, long runeId, ScoreType type);
		void SetRanks(List<ScoreRankingResult> ranks);
		void AddOrUpdateRanks(List<ScoreRankingResult> ranks);
	}
}
