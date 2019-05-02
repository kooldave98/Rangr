using System;

namespace App.Library.EntityFramework.Contexts.ConnectionStringProviders {

    /// <summary>
    ///     <see cref="IConnectionStringProvider"/> that gets the connection string
    /// from an environment variable.
    /// </summary>
    public class EnvironmentVariableConnectionStringProvider 
                    : IConnectionStringProvider {

        /// <summary>
        ///     The value of the environment variable or an empty string if the 
        /// environment variable does not exist.
        /// </summary>
        public string connection_string {
            
            get { return Environment.GetEnvironmentVariable( environment_variable ) ?? ""; }
        }

        /// <summary>
        ///     Accepts the name of the name of environment variable that holds the connection string
        /// </summary>
        /// <param name="the_environment_variable">
        ///     the name of the environment variable that holds the connection string.
        /// </param>
        public EnvironmentVariableConnectionStringProvider 
                ( string the_environment_variable ) {
            
            environment_variable = the_environment_variable;
        }

        // name of the environment variable that holds the connection string
        private readonly string environment_variable;
    }

}