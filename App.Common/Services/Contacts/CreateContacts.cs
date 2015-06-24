using System;

namespace App.Common
{
    public class Contacts
    {
        public Contacts()
        {
        }
    }

    public class CreateContactsRequest : UserIdentity
    {
        public long[] mobile_numbers { get; set; }
    }

    public class RemoveContactsRequest : UserIdentity
    {
        public long[] mobile_numbers { get; set; }
    }
}

