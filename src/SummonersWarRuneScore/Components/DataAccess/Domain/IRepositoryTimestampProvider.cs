using System;

namespace SummonersWarRuneScore.Components.DataAccess.Domain
{
	public interface IRepositoryTimestampProvider
	{
		DateTime GetResourceLastWriteTime();
	}
}
