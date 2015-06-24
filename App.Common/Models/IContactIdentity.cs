using System;

namespace App.Common
{
    public class ContactIdentity : BaseEntity, IContactIdentity
    {
        public long owning_user_id { get; set; }

        public long underlying_user_id { get; set; }
    }

    public interface IContactIdentity
    {
        long owning_user_id { get; set; }

        long underlying_user_id { get; set; }
    }
}

