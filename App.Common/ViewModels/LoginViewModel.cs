using System;
using System.Threading.Tasks;

namespace App.Common
{
    public class LoginViewModel : ViewModelBase
    {
        public string user_mobile_number { get; set; }

        public async Task<bool> CreateUser()
        {
            long val = 0;
            if (!long.TryParse(user_mobile_number, out val))
            {
                throw new InvalidOperationException("You must enter a valid mobile number to create a new user");
            }

            var userID = await create_user_service.Create(new CreateUserRequest()
                { 
                    mobile_number = val
                });

            if (userID == null)
            {
                return false;
            }

            var user = new User()
            { 
                user_id = userID.user_id
            };

            //var user = await get_user_by_id.Get (new UserIdentity(){user_id = userID.user_id});

            if (user == null)
            {
                return false;
            }

            sessionInstance.PersistCurrentUser(user);


            return true;
        }

        public LoginViewModel()
        {
            sessionInstance = Session.Current;

            create_user_service = new CreateUser();
            get_user_by_id = new GetUserById();
        }

        private CreateUser create_user_service;
        private GetUserById get_user_by_id;
        private Session sessionInstance;
    }
}

