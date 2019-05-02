
namespace App.Library.CodeStructures.Behavioral
{
    /// <summary>
    ///     Definition of a service layer command.
    /// </summary>
    /// <typeparam name="P">
    ///     Request type passed into the execute method.
    /// </typeparam>
    public interface ICommand<Q>
    {

        /// <summary>
        ///     Invokes the command with the arguments specified in the request.
        /// </summary>
        Q execute();

    }


    /// <summary>
    ///     Definition of a service layer command.
    /// </summary>
    /// <typeparam name="P">
    ///     Request type passed into the execute method.
    /// </typeparam>
    /// <typeparam name="Q">
    ///     Return type of the command
    /// </typeparam>
    public interface ICommand<P, Q>
    {

        /// <summary>
        ///     Invokes the command with the parameters specified in the request.
        /// </summary>
        /// <param name="request">
        ///     arguments for the command to work on.
        /// </param>
        Q execute(P request);

    }
}
