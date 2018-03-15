using System.Collections.Generic;
using SummonersWarRuneScore.Components.Domain;

namespace SummonersWarRuneScore.Components.RuneScoring
{
	public interface IRuneScoreCache
	{
		RuneScoringResult GetScore(int roleId, long runeId);
		void SetScores(List<RuneScoringResult> runeScores);
		void AddOrUpdateScores(List<RuneScoringResult> runeScores);
	}
}
