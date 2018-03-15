using SummonersWarRuneScore.Components.Domain.Enumerations;

namespace SummonersWarRuneScore.Components.Domain
{
	public class ScoreRankingResult
	{
		// Meta data
		public int RoleId { get; private set; }
		public long RuneId { get; private set; }
		public ScoreType ScoreType { get; private set; }

		public int Rank { get; private set; }

		public ScoreRankingResult(int roleId, long runeId, ScoreType scoreType, int rank)
		{
			RoleId = roleId;
			RuneId = runeId;
			ScoreType = scoreType;
			Rank = rank;
		}
	}
}
