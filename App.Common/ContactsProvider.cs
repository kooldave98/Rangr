using System.Linq;

using XContact = Xamarin.Contacts.Contact;
using XAddressBook = Xamarin.Contacts.AddressBook;
using System.Threading.Tasks;
using common_lib;
using System.Collections.Generic;

#if __ANDROID__
using Android.App;
#else

#endif


namespace App.Common
{
    public class ContactsProvider : SingletonBase<ContactsProvider>
    {
        private IEnumerable<NamedNumber> cached_data;

        public async Task<IEnumerable<NamedNumber>> get_contacts()
        {            
            if (cached_data == null)
            {
                var permission_granted = await address_book.RequestPermission();

                if (!permission_granted)
                    return Enumerable.Empty<NamedNumber>();
            

                var data = address_book
                    .ToList();

                cached_data= data
                .Where(c => (!string.IsNullOrWhiteSpace(c.LastName) || !string.IsNullOrWhiteSpace(c.FirstName)) && c.Phones.Count() > 0)
                .SelectMany(c => c.Phones.Select(p => new NamedNumber(){ Name = string.Format("{0} {1}", c.FirstName, c.LastName), Number = get_clean_number(p.Number) }));
            }

            return cached_data;        
        }

        public void invalidate_cache()
        {
            cached_data = null;
        }

        public async Task<string> get_name_for_number(string mobile_number)
        {
            var clean_number = get_clean_number(mobile_number);

            if (clean_number == user_id)
                return "Me";
            

            var found = (await get_contacts()).SingleOrDefault(c => c.Number == clean_number);

            if (found != null)
            {
                return found.Name;
            }

            return clean_number;
        }

        private string user_id {
            get{ return Session.Current.GetCurrentUser().user_id; }
        }

        private string get_clean_number(string mobile_number)
        {
            var code = PhoneNumberHelper.Current.get_2L_country_code_for_valid_international_number(Session.Current.GetCurrentUser().user_id);
            return PhoneNumberFormatter.Current.clean_number(mobile_number, code);
        }

        private ContactsProvider() { 
            /*This is important for the singleton base to work.*/ 
            #if __ANDROID__
            address_book = new XAddressBook (Application.Context); //on Android
            #else
            address_book = new XAddressBook ();
            #endif
        }

        private XAddressBook address_book;
    }


    public class NamedNumber
    {
        public string Name { get; set; }

        public string Number { get; set;}
    }

}

