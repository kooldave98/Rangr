namespace App.Library.Persistence
{
    /// <summary>
    ///    Base Entity that all App entities will inherit from
    /// </summary>
    /// <typeparam name="I">
    ///     The entities Identity type.
    /// </typeparam>
    public class BaseEntity<I>
    {

        /// <summary>
        ///     The unique identity of the the entity
        /// </summary>
        public virtual I ID { get; set; }

    }
}
