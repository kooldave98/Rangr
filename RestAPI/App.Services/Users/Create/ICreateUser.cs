
using App.Library.CodeStructures.Behavioral;

namespace App.Services.Users.Create
{
    public interface ICreateUser : ICommand<CreateUserRequest, UserIdentity>
    {
        
    }
}
