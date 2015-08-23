namespace App.Persistence.EF.Infrastructure {

    /// <summary>
    ///     Setting required by context to connect to the server
    /// </summary>
    public interface IConnectionSettings {

        string ConnectionString { get; }  

    }

}