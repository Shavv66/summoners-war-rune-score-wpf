using System;

namespace SummonersWarRuneScore.Components.Domain
{
	public enum OperatorType
	{
		Equal,
		NotEqual,
		GreaterThan,
		LessThan,
		GreaterThanOrEqual,
		LessThanOrEqual,
		Contains,
		DoesNotContain,
		StartsWith,
		EndsWith
	}

	public class FilterOperator
	{
		public OperatorType Type { get; private set; }

		public FilterOperator(OperatorType type)
		{
			Type = type;
		}

		public bool Apply(object leftOperand, object rightOperand)
		{
			switch(Type)
			{
				case OperatorType.Equal: return ApplyEqual(leftOperand, rightOperand);
				case OperatorType.NotEqual: return ApplyNotEqual(leftOperand, rightOperand);
				case OperatorType.GreaterThan: return ApplyGreaterThan(leftOperand, rightOperand);
				case OperatorType.LessThan: return ApplyLessThan(leftOperand, rightOperand);
				case OperatorType.GreaterThanOrEqual: return ApplyGreaterThanOrEqual(leftOperand, rightOperand);
				case OperatorType.LessThanOrEqual: return ApplyLessThanOrEqual(leftOperand, rightOperand);
				case OperatorType.Contains: return ApplyContains(leftOperand, rightOperand);
				case OperatorType.DoesNotContain: return ApplyDoesNotContain(leftOperand, rightOperand);
				case OperatorType.StartsWith: return ApplyStartsWith(leftOperand, rightOperand);
				case OperatorType.EndsWith: return ApplyEndsWith(leftOperand, rightOperand);
				default: throw new Exception($"Undefined filter operator type: {Type}");
			}
		}

		private bool ApplyEqual(object leftOperand, object rightOperand)
		{
			return leftOperand.Equals(rightOperand);
		}

		private bool ApplyNotEqual(object leftOperand, object rightOperand)
		{
			return !ApplyEqual(leftOperand, rightOperand);
		}

		private bool ApplyGreaterThan(object leftOperand, object rightOperand)
		{
			return TryParseComparable(leftOperand).CompareTo(TryParseComparable(rightOperand)) > 0;
		}

		private bool ApplyLessThan(object leftOperand, object rightOperand)
		{
			return TryParseComparable(leftOperand).CompareTo(TryParseComparable(rightOperand)) < 0;
		}

		private bool ApplyGreaterThanOrEqual(object leftOperand, object rightOperand)
		{
			return ApplyEqual(leftOperand, rightOperand) || ApplyGreaterThan(leftOperand, rightOperand);
		}

		private bool ApplyLessThanOrEqual(object leftOperand, object rightOperand)
		{
			return ApplyEqual(leftOperand, rightOperand) || ApplyLessThan(leftOperand, rightOperand);
		}

		private bool ApplyContains(object leftOperand, object rightOperand)
		{
			return leftOperand.ToString().Contains(rightOperand.ToString());
		}

		private bool ApplyDoesNotContain(object leftOperand, object rightOperand)
		{
			return !ApplyContains(leftOperand, rightOperand);
		}

		private bool ApplyStartsWith(object leftOperand, object rightOperand)
		{
			return leftOperand.ToString().StartsWith(rightOperand.ToString());
		}

		private bool ApplyEndsWith(object leftOperand, object rightOperand)
		{
			return leftOperand.ToString().EndsWith(rightOperand.ToString());
		}

		private IComparable TryParseComparable(object operand)
		{
			if (!(operand is IComparable comparable))
			{
				throw new ArgumentException($"Filter operator '{Type}' cannot be applied to type '{operand.GetType().Name}'");
			}

			return comparable;
		}
	}
}
