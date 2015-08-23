
namespace App.Service.Framework.Queries
{

    // Usage note - Each query is intended as a very thin interface with only one 
    // execute method please do not add extra methods to this interface.  

    /// <summary>
    /// Definition of a service layer query.
    /// </summary>
    /// <typeparam name="D">Details returned by the query</typeparam>
    public interface IQuery<D>
    {

        /// <summary>
        /// Executes the query returning the result set
        /// </summary>
        D Execute();
    }

    /// <summary>
    /// Definition of a service layer parameterised query.
    /// </summary>
    /// <typeparam name="D">The return type of the query</typeparam>
    /// <typeparam name="R">Request type passed into the execute method.</typeparam>
    public interface IQuery<D, R>
    {

        /// <summary>
        /// Executes the query with the parameters specified in the request.
        /// </summary>
        /// <param name="request">The request</param>
        D Execute(R request);
    }
}

