using SummonersWarRuneScore.Domain.Enumerations;
using System.Collections.Generic;

namespace SummonersWarRuneScore.Domain
{
	public interface IRuneScoreCache
	{
		RuneScoringResult GetScore(RuneSet set, string roleName, long runeId);
		void SetScores(List<RuneScoringResult> runeScores);
		void AddOrUpdateScores(List<RuneScoringResult> runeScores);
	}
}