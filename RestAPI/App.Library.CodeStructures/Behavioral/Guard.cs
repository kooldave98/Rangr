using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Library.CodeStructures.Behavioral
{
    /// <summary>
    ///     Contains guard conditions (pre, post parameter check) tests
    /// </summary>
    public class Guard
    {

        /// <summary>
        ///     Throws an ArgumentNullException if the argument is null
        /// </summary>
        /// <param name="argument">The argument to check.</param>
        /// <param name="argument_name">The name of the argument.</param>
        /// <exception cref="ArgumentNullException">If the argument is null.</exception>
        public static void IsNotNull(object argument, string argument_name)
        {

            if (argument == null)
            {
                throw new ArgumentNullException(argument_name);
            }
        }

        public static S IsNotNull<S>(S argument, string argument_name)
        {

            if (argument == null)
            {
                throw new ArgumentNullException(argument_name);
            }
            return argument;
        }


        public static void IsNull(object argument, string argument_name)
        {

            if (argument != null)
            {
                throw new ArgumentNullException(argument_name);
            }
        }

        /// <summary>
        ///     Checks that the predicate is true, if it is not then
        /// an <see cref="ArgumentException"/> is thrown
        /// </summary>
        /// <param name="predicate">
        ///     The expression that is expected to be true
        /// </param>
        /// <param name="error_message">
        ///    The message that the exception will be set to report.
        /// </param>
        public static void PredicateHolds
                            (bool predicate
                            , string error_message)
        {

            if (!predicate)
            {
                throw new ArgumentException(error_message);
            }

        }

    }

}
