using System;
using NUnit.Framework;

namespace RestMvc.UnitTests.Assertions
{
    public static class CustomAssert
    {
        public static void That(Action action, LambdaConstraint constraint)
        {
            if (constraint.Matches(action))
                return;

            var writer = new TextMessageWriter();
            constraint.WriteMessageTo(writer);
            throw new AssertionException(writer.ToString());
        }
    }
}
