namespace App.Library.CodeStructures.Behavioral
{
    /// <summary>
    ///     Query does not require a parameters to be invoked.
    /// </summary>
    /// <typeparam name="Q">Details returned by the query</typeparam>
    public interface IQuery<Q>
    {

        /// <summary>
        ///     Executes the query returning the result set
        /// </summary>
        Q execute();
    }


    /// <summary>
    /// Definition of a service layer parameterised query.
    /// </summary>
    /// <typeparam name="R">Request type passed into the execute method.</typeparam>
    /// <typeparam name="Q">The return type of the query</typeparam>
    public interface IQuery<R, Q>
    {

        /// <summary>
        ///     Executes the query with the specified argument.
        /// </summary>
        /// <param name="request">
        ///     The request
        /// </param>
        Q execute
            (R request);
    }
}
