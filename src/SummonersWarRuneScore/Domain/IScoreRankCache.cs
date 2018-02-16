using SummonersWarRuneScore.Domain.Enumerations;
using System.Collections.Generic;

namespace SummonersWarRuneScore.Domain
{
	public interface IScoreRankCache
	{
		ScoreRankingResult GetRank(int roleId, long runeId, ScoreType type);
		void SetRanks(List<ScoreRankingResult> ranks);
		void AddOrUpdateRanks(List<ScoreRankingResult> ranks);
	}
}
