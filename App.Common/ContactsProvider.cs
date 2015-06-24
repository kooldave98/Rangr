using System.Linq;

using XamAddressBook = Xamarin.Contacts.AddressBook;
using XamContact = Xamarin.Contacts.Contact;
using System.Threading.Tasks;

#if __ANDROID__
using Android.App;
#else

#endif


namespace App.Common
{
    public class ContactsProvider : SingletonBase<ContactsProvider>
    {
        public void get_contacts()
        {
            foreach (XamContact contact in address_book.OrderBy (c => c.LastName)) {
                //Console.WriteLine ("{0} {1}", contact.FirstName, contact.LastName);
            }
        }

        public async Task<string> get_name_for_number(long mobile_number)
        {
            if (! await request_permission())
            {
                return mobile_number.ToString();
            }

//            if (mobile_number == user_id)
//            {
//                return "Me";
//            }

            foreach (var contact in address_book) 
            {
                foreach (var number in contact.Phones)
                {
                    if (number.Number == "+" + mobile_number.ToString())
                    {
                        return contact.FirstName + contact.LastName;
                    }
                }
            }

            return mobile_number.ToString();
        }

        public async Task<bool> request_permission()
        {
            permission_granted = await address_book.RequestPermission(); 

            return permission_granted;
        }

        private ContactsProvider()
        {
            #if __ANDROID__
            address_book = new XamAddressBook (Application.Context); //on Android
            #else
            address_book = new XamAddressBook ();
            #endif

            user_id = Session.Current.GetCurrentUser().user_id;
        }

        private XamAddressBook address_book;
        private long user_id;
        private bool permission_granted = false;
    }
}

