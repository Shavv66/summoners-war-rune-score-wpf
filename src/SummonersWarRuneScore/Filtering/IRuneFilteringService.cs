using SummonersWarRuneScore.Domain;
using System.Collections.Generic;

namespace SummonersWarRuneScore.Filtering
{
	public interface IRuneFilteringService
    {
		List<Rune> FilterRunes(List<Rune> runes, Filter filter);
    }
}
