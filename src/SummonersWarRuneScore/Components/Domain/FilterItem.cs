using SummonersWarRuneScore.Components.Domain.Enumerations;

namespace SummonersWarRuneScore.Components.Domain
{
	public class FilterItem : IFilter
    {
		public RuneFilterProperty Property { get; private set; }
		public FilterOperator Operator { get; private set; }
		public object Value { get; private set; }

		public FilterItem(RuneFilterProperty property, OperatorType @operator, object value)
		{
			Property = property;
			Operator = new FilterOperator(@operator);
			Value = value;
		}
    }
}
