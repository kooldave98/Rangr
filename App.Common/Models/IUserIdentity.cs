using System;

namespace App.Common
{
    public class UserIdentity : BaseEntity, IUserIdentity
    {
        public string user_id { get; set; }
    }


    public interface IUserIdentity
    {
        string user_id { get; set; }
    }
}

