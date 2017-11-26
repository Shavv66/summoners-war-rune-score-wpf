using SummonersWarRuneScore.Domain.Enumerations;
using System.Collections.Generic;

namespace SummonersWarRuneScore.Domain
{
	public class RuneScoreCache : IRuneScoreCache
	{
		// Scores are stored in nested dictionaries to allow for fast cache lookups
		private Dictionary<RuneSet, Dictionary<string, Dictionary<long, RuneScoringResult>>> mRuneScores;

		public RuneScoringResult GetScore(RuneSet set, string roleName, long runeId)
		{
			return mRuneScores[set][roleName][runeId];
		}

		public void SetScores(List<RuneScoringResult> runeScores)
		{
			mRuneScores = new Dictionary<RuneSet, Dictionary<string, Dictionary<long, RuneScoringResult>>>();
			AddOrUpdateScores(runeScores);
		}

		public void AddOrUpdateScores(List<RuneScoringResult> runeScores)
		{
			foreach (RuneScoringResult runeScore in runeScores)
			{
				if (!mRuneScores.ContainsKey(runeScore.RuneSet))
				{
					mRuneScores.Add(runeScore.RuneSet, new Dictionary<string, Dictionary<long, RuneScoringResult>>());
				}
				Dictionary<string, Dictionary<long, RuneScoringResult>> scoresForSet = mRuneScores[runeScore.RuneSet];

				if (!scoresForSet.ContainsKey(runeScore.RoleName))
				{
					scoresForSet.Add(runeScore.RoleName, new Dictionary<long, RuneScoringResult>());
				}
				Dictionary<long, RuneScoringResult> scoresForRole = scoresForSet[runeScore.RoleName];

				if (!scoresForRole.ContainsKey(runeScore.RuneId))
				{
					scoresForRole.Add(runeScore.RuneId, runeScore);
				}
				else
				{
					scoresForRole[runeScore.RuneId] = runeScore;
				}
			}
		}
	}
}
