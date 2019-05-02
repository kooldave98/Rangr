namespace App.Library.Persistence
{
    /// <summary>
    ///     Variant of the Unit of Work (PEAA) pattern
    /// </summary>
    public interface IUnitOfWork
    {

        /// <summary>
        ///     Commits all changes to persistent storage
        /// </summary>
        void Commit();

    }
}
