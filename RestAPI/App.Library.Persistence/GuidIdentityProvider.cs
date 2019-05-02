using System;

namespace App.Library.Persistence
{
    public class GuidIdentityProvider
    {
        public static Guid next_id()
        {
            return Guid.NewGuid();
        }
    }
}