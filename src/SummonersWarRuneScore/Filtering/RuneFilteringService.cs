using SummonersWarRuneScore.Domain;
using SummonersWarRuneScore.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SummonersWarRuneScore.Filtering
{
	public class RuneFilteringService : IRuneFilteringService
    {
		public List<Rune> FilterRunes(List<Rune> runes, Filter filter)
		{
			return runes.Where(rune => RuneIsInFilter(rune, filter)).ToList();
		}

		private bool RuneIsInFilter(Rune rune, Filter filter)
		{
			foreach (IFilter innerFilter in filter.Filters)
			{
				bool matchesInnerFilter;
				if (innerFilter is Filter)
				{
					matchesInnerFilter = RuneIsInFilter(rune, (Filter)innerFilter);
				}
				else
				{
					FilterItem filterItem = innerFilter as FilterItem;
					object runeValue = GetRuneValueFromProperty(rune, filterItem.Property);
					matchesInnerFilter = filterItem.Operator.Apply(runeValue, filterItem.Value);
				}

				if (matchesInnerFilter)
				{
					if (filter.Logic == FilterLogic.Or)
					{
						return true;
					}
				}
				else if (filter.Logic == FilterLogic.And)
				{
					return false;
				}
			}

			return filter.Logic == FilterLogic.And;
		}

		private object GetRuneValueFromProperty(Rune rune, RuneFilterProperty propertyName)
		{
			switch (propertyName)
			{
				case RuneFilterProperty.Id: return rune.Id;
				case RuneFilterProperty.SummonerId: return rune.SummonerId;
				case RuneFilterProperty.Location: return rune.Location;
				case RuneFilterProperty.MonsterId: return rune.MonsterId;
				case RuneFilterProperty.Slot: return rune.Slot;
				case RuneFilterProperty.Colour: return rune.Colour;
				case RuneFilterProperty.Stars: return rune.Stars;
				case RuneFilterProperty.Set: return rune.Set;
				case RuneFilterProperty.Level: return rune.Level;
				case RuneFilterProperty.SellValue: return rune.SellValue;
				case RuneFilterProperty.PrimaryStatType: return rune.PrimaryStat.Type;
				case RuneFilterProperty.PrimaryStatAmount: return rune.PrimaryStat.Amount;
				case RuneFilterProperty.PrefixStatType: return rune.PrefixStat.Type;
				case RuneFilterProperty.PrefixStatAmount: return rune.PrefixStat.Amount;
				case RuneFilterProperty.Quality: return rune.Quality;
				default: throw new ArgumentException($"{propertyName} is not a valid property name for rune filtering");
			}
		}
    }
}
