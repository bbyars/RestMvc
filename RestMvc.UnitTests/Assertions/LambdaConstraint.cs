using System;
using NUnit.Framework.Constraints;

namespace RestMvc.UnitTests.Assertions
{
    public abstract class LambdaConstraint : Constraint
    {
        public bool Matches(Action action)
        {
            return Matches((object)action);
        }
    }
}
