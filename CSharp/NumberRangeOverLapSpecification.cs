namespace NumberGenerator.Core
{
   using System;

    public class NumberRangeOverLapSpecification : BaseSpecification<NumberRange>
    {
        public NumberRangeOverLapSpecification(NumberRange newNumberRange) : base(r =>
                Math.Max(r.RangeFrom, newNumberRange.RangeFrom) - Math.Min(r.RangeTo, newNumberRange.RangeTo) <= 0)
        {
        }
    }
}
