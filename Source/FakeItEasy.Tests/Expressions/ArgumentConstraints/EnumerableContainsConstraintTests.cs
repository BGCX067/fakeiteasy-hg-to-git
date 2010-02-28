namespace FakeItEasy.Tests.Expressions.ArgumentConstraints
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;
    using FakeItEasy.Expressions.ArgumentConstraints;
    using System.Linq;
    using FakeItEasy.Expressions;
    using System.Collections;
    
    [TestFixture]
    public class EnumerableContainsConstraintTests
        : ArgumentConstraintTestBase<IEnumerable>
    {
        [SetUp]
        public void SetUp()
        {
            var scope = A<IEnumerable>.That;
            
            this.constraint = new EnumerableContainsConstraint(scope, 10);
        }

        protected override IEnumerable<object> InvalidValues
        {
            get
            {
                return new List<List<object>>()
                {
                    new List<object>() { 1, 2, 3, 4 },
                    new List<object>(),
                    new List<object>() { 9, 11 },
                    null
                }.Cast<object>();
            }
        }

        protected override IEnumerable<object> ValidValues
        {
            get 
            {
                return new List<List<object>>() 
                {
                    new List<object>() { 10 },
                    new List<object>() { 9, 10, 11 },
                    new List<object>() { null, 10, 90 }
                }.Cast<object>();
            }
        }

        protected override string ExpectedDescription
        {
            get 
            {
                return "contains 10";
            }
        }
    }
}
