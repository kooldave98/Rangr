namespace App.Library.EntityFramework.Contexts.TestConnections
{
    public interface ICanConnectToDatabase
    {
        CanConnectToDatabaseResponse verify
                                        (string conection_string);
    }
}