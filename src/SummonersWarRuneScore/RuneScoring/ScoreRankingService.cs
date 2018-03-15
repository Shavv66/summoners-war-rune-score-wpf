using System;
using System.Collections.Generic;
using SummonersWarRuneScore.Components.Domain;
using SummonersWarRuneScore.Components.Domain.Enumerations;

namespace SummonersWarRuneScore.RuneScoring
{
	public class ScoreRankingService : IScoreRankingService
	{
		public List<ScoreRankingResult> CalculateRanks(List<RuneScoringResult> scores)
		{
			// First assemble lists of scores by the attributes which make up separate ranking classes
			Dictionary<int, Dictionary<ScoreType, List<Tuple<long, decimal>>>> scoresByRoleAndType = new Dictionary<int, Dictionary<ScoreType, List<Tuple<long, decimal>>>>();
			foreach (RuneScoringResult score in scores)
			{
				if (!scoresByRoleAndType.ContainsKey(score.RoleId))
				{
					scoresByRoleAndType.Add(score.RoleId, new Dictionary<ScoreType, List<Tuple<long, decimal>>>());
				}
				Dictionary<ScoreType, List<Tuple<long, decimal>>> scoresForRole = scoresByRoleAndType[score.RoleId];

				foreach (ScoreType type in Enum.GetValues(typeof(ScoreType)))
				{
					if (!scoresForRole.ContainsKey(type))
					{
						scoresForRole.Add(type, new List<Tuple<long, decimal>>());
					}
					List<Tuple<long, decimal>> scoresForRoleAndType = scoresForRole[type];

					scoresForRoleAndType.Add(new Tuple<long, decimal>(score.RuneId, score.GetScore(type)));
				}
			}

			List<ScoreRankingResult> ranks = new List<ScoreRankingResult>();

			// Now we have our lists we can sort them to obtain ranks
			foreach (int roleId in scoresByRoleAndType.Keys)
			{
				foreach (ScoreType type in scoresByRoleAndType[roleId].Keys)
				{
					List<Tuple<long, decimal>> scoresForRoleAndType = scoresByRoleAndType[roleId][type];
					scoresForRoleAndType.Sort((first, second) => -first.Item2.CompareTo(second.Item2));
					for (int index = 0; index < scoresForRoleAndType.Count; index++)
					{
						int rank = index + 1;
						long runeId = scoresForRoleAndType[index].Item1;
						ranks.Add(new ScoreRankingResult(roleId, runeId, type, rank));
					}
				}
			}

			return ranks;
		}
	}
}
