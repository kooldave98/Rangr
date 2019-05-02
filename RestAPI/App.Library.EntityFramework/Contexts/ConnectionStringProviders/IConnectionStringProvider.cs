namespace App.Library.EntityFramework.Contexts.ConnectionStringProviders {

    /// <summary>
    ///     Provider that supplies an connection string to an Database context
    /// </summary>
    public interface IConnectionStringProvider {

        /// <summary>
        ///     The context's connections string       
        /// </summary>
        string connection_string { get; } 

    }

}