using System;
using System.Threading.Tasks;
using common_lib;

namespace App.Common
{
    public class LoginViewModel : ViewModelBase
    {
        public string user_mobile_number { get; set; }

        public async Task<bool> CreateUser()
        {
            //TODO: Validate mobile number

            var userID = await create_user_service.Create(new CreateUserRequest()
                { 
                    mobile_number = user_mobile_number
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

