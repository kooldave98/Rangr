

namespace App.Library.CodeStructures.Behavioral
{
    public interface IPolicy<C>
    {

        bool decide_for(C context);

    }
}
