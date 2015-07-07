using System;
using common_lib;

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
