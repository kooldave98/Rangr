
namespace App.Service.Framework.Commands
{
        // Usage note - This is intended as a very thin interface with only one 
        // execute method please do not add extra methods to this interface.  

        /// <summary>
        /// Definition of a service layer command.
        /// </summary>
        /// <typeparam name="R">Request type passed into the execute method.</typeparam>
        public interface ICommand<R>
        {

            /// <summary>
            /// Invokes the command with the parameters specified in the request.
            /// </summary>
            /// <param name="request">The request.</param>
            void Execute(R request);
        }

        /// <summary>
        /// Definition of a service layer command.
        /// </summary>
        /// <typeparam name="R">Request type passed into the execute method.</typeparam>
        /// <typeparam name="D">Return type of the command</typeparam>
        public interface ICommand<R, D>
        {

            /// <summary>
            /// Invokes the command with the parameters specified in the request.
            /// </summary>
            /// <param name="request">The request.</param>
            D Execute(R request);
        }
    }
