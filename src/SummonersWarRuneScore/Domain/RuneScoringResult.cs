using SummonersWarRuneScore.Domain.Enumerations;

namespace SummonersWarRuneScore.Domain
{
	public class RuneScoringResult
	{
		// Meta data
		public RuneSet RuneSet { get; private set; }
		public string RoleName { get; private set; }
		public long RuneId { get; private set; }

		// Scores
		public decimal CurrentScore { get; private set; }

		public RuneScoringResult(RuneSet runeSet, string roleName, long runeId, decimal currentScore)
		{
			RuneSet = runeSet;
			RoleName = roleName;
			RuneId = runeId;
			CurrentScore = currentScore;
		}
	}
}
