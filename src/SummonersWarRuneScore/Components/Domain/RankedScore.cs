using System;

namespace SummonersWarRuneScore.Components.Domain
{
	public class RankedScore : IComparable
	{
		private readonly decimal mScore;
		private readonly int mRank;

		public RankedScore(decimal score, int rank)
		{
			mScore = score;
			mRank = rank;
		}

		public int CompareTo(object other)
		{
			if (!(other is RankedScore otherScore))
			{
				return 0;
			}

			return mScore.CompareTo(otherScore.mScore);
		}

		public override string ToString()
		{
			return $"{mScore} ({mRank})";
		}
	}
}
