using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace RestMvc.UnitTests.Assertions
{
    public class Throws<TException> : LambdaConstraint where TException : Exception
    {
        private readonly List<KeyValuePair<Func<TException, object>, Constraint>> assertions
            = new List<KeyValuePair<Func<TException, object>, Constraint>>();

        public static Throws<TException> Where(Func<TException, object> actual, Constraint expected)
        {
            return new Throws<TException>().And(actual, expected);
        }

        public virtual Throws<TException> And(Func<TException, object> actual, Constraint expected)
        {
            assertions.Add(new KeyValuePair<Func<TException, object>, Constraint>(actual, expected));
            return this;
        }

        public override bool Matches(object actual)
        {
            var test = (Action)actual;
            try
            {
                test();
                return false;
            }
            catch (TException ex)
            {
                foreach (var assertion in assertions)
                    Assert.That(assertion.Key(ex), assertion.Value);

                return true;
            }
        }

        public override void WriteDescriptionTo(MessageWriter writer)
        {
            writer.Write("Should have thrown " + typeof(TException).Name);
        }
    }
}
