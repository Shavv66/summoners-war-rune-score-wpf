using SummonersWarRuneScore.Components.Domain;
using System.Collections.Generic;

namespace SummonersWarRuneScore.Client.UserControls.RuneScoringGrid.Domain
{
	public class RuneScoringGridData
	{
		public IReadOnlyCollection<MonsterRole> MonsterRoles { get; set; }
		public IReadOnlyCollection<Rune> Runes { get; set; }
	}
}
