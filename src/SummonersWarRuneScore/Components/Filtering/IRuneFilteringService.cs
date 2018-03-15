using System.Collections.Generic;
using SummonersWarRuneScore.Components.Domain;

namespace SummonersWarRuneScore.Components.Filtering
{
	public interface IRuneFilteringService
    {
		List<Rune> FilterRunes(List<Rune> runes, Filter filter);
    }
}
