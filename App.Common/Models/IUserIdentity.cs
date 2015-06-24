using System;

namespace App.Common
{
    public class UserIdentity : BaseEntity, IUserIdentity
    {
        public long user_id { get; set; }
    }


    public interface IUserIdentity
    {
        long user_id { get; set; }
    }
}

