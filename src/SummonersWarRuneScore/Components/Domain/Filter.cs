using System.Collections.Generic;
using SummonersWarRuneScore.Components.Domain.Enumerations;

namespace SummonersWarRuneScore.Components.Domain
{
	public class Filter : IFilter
    {
		public List<IFilter> Filters { get; private set; }
		public FilterLogic Logic { get; private set; }

		public Filter(List<IFilter> filters, FilterLogic logic)
		{
			Filters = filters;
			Logic = logic;
		}
    }
}
