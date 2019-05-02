
using App.Library.CodeStructures.Behavioral;

namespace App.Services.Users.Update
{
    public interface IUpdateUser : ICommand<UpdateUserRequest, UserIdentity>
    {
        
    }

}
