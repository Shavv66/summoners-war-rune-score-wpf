using System.Collections.Generic;
using SummonersWarRuneScore.Components.Domain;
using SummonersWarRuneScore.Components.Domain.Enumerations;

namespace SummonersWarRuneScore.Components.RuneScoring
{
	public class ScoreRankCache : IScoreRankCache
	{
		private Dictionary<int, Dictionary<long, Dictionary<ScoreType, ScoreRankingResult>>> mScoreRanks;

		public ScoreRankingResult GetRank(int roleId, long runeId, ScoreType type)
		{
			return mScoreRanks[roleId][runeId][type];
		}

		public void SetRanks(List<ScoreRankingResult> ranks)
		{
			mScoreRanks = new Dictionary<int, Dictionary<long, Dictionary<ScoreType, ScoreRankingResult>>>();
			AddOrUpdateRanks(ranks);
		}

		public void AddOrUpdateRanks(List<ScoreRankingResult> ranks)
		{
			foreach (ScoreRankingResult rank in ranks)
			{
				if (!mScoreRanks.ContainsKey(rank.RoleId))
				{
					mScoreRanks.Add(rank.RoleId, new Dictionary<long, Dictionary<ScoreType, ScoreRankingResult>>());
				}
				Dictionary<long, Dictionary<ScoreType, ScoreRankingResult>> ranksForRole = mScoreRanks[rank.RoleId];

				if (!ranksForRole.ContainsKey(rank.RuneId))
				{
					ranksForRole.Add(rank.RuneId, new Dictionary<ScoreType, ScoreRankingResult>());
				}
				Dictionary<ScoreType, ScoreRankingResult> ranksForRune = ranksForRole[rank.RuneId];

				if (!ranksForRune.ContainsKey(rank.ScoreType))
				{
					ranksForRune.Add(rank.ScoreType, rank);
				}
				else
				{
					ranksForRune[rank.ScoreType] = rank;
				}
			}
		}
	}
}
