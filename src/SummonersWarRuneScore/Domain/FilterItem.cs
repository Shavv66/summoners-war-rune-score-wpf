using SummonersWarRuneScore.Domain.Enumerations;

namespace SummonersWarRuneScore.Domain
{
	public class FilterItem : IFilter
    {
		public string PropertyName { get; private set; }
		public FilterOperator Operator { get; private set; }
		public object Value { get; private set; }

		public FilterItem(string propertyName, FilterOperator @operator, object value)
		{
			PropertyName = propertyName;
			Operator = @operator;
			Value = value;
		}
    }
}
