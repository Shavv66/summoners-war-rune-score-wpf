using SummonersWarRuneScore.Domain.Enumerations;
using System.Collections.Generic;

namespace SummonersWarRuneScore.Domain
{
	public interface IRuneScoreCache
	{
		RuneScoringResult GetScore(int roleId, long runeId);
		void SetScores(List<RuneScoringResult> runeScores);
		void AddOrUpdateScores(List<RuneScoringResult> runeScores);
	}
}