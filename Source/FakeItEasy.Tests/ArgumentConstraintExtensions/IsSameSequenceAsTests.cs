namespace FakeItEasy.Tests.ArgumentValidationExtensions
{
    using System.Collections.Generic;
    using NUnit.Framework;

    [TestFixture]
    public class IsSameSequenceAsTests
        : ArgumentConstraintTestBase<IEnumerable<int>>
    {
        protected override IEnumerable<object> InvalidValues
        {
            get
            {
                yield return new int[] { 1, 2 };
                yield return new int[] { };
                yield return null;
                yield return new int[] { 1, 2, 3, 4 };
                yield return new int[] { 9, 8 };
            }
        }

        protected override IEnumerable<object> ValidValues
        {
            get
            {
                yield return new int[] { 1, 2, 3 };
                yield return new List<int> { 1, 2, 3 };
            }
        }

        protected override void CreateConstraint(FakeItEasy.Core.IArgumentConstraintManager<IEnumerable<int>> scope)
        {
            scope.IsSameSequenceAs(new int[] {1, 2, 3});
        }

        protected override string ExpectedDescription
        {
            get
            {
                return "specified sequence";
            }
        }
    }
}
