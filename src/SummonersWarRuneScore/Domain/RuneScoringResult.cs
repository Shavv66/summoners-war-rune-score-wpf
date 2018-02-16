using SummonersWarRuneScore.Domain.Enumerations;
using System.Collections.Generic;

namespace SummonersWarRuneScore.Domain
{
	public class RuneScoringResult
	{
		// Meta data
		public int RoleId { get; private set; }
		public long RuneId { get; private set; }

		// Scores
		private Dictionary<ScoreType, decimal> Scores { get; set; }

		public RuneScoringResult(int roleId, long runeId, decimal currentScore)
		{
			RoleId = roleId;
			RuneId = runeId;

			Scores = new Dictionary<ScoreType, decimal>();
			Scores[ScoreType.Current] = currentScore;
		}

		public decimal GetScore(ScoreType type)
		{
			return Scores[type];
		}
	}
}
