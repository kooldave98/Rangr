namespace App.Library.CodeStructures.Creational {

    /// <summary>
    ///     Used to create requests for items that do not 
    /// require information from an existing entity.
    /// </summary>
    /// <typeparam name="Q">
    ///     The type of request that factory is expected to return.
    /// </typeparam>
    public interface IFactory<Q> {

        Q create ();
    }


    /// <summary>
    ///     Used to create requests for items that require information 
    /// from an existing entity.  It is expected that the request will
    /// always be an identity although this has not been enforced.
    /// </summary>
    /// <typeparam name="P">
    ///     The type of the argument needed to create a request.
    /// </typeparam>
    /// <typeparam name="Q">
    ///     The type of request that factory is expected to return.
    /// </typeparam>
    public interface IFactory<P,Q> {
        
        Q create
            ( P request );

    }
}