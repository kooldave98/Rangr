using System;

namespace common_lib
{
    public static class GeneralExtensions
    {        
        public static T Chain<T>(this T @this, Action<T> initAction)
        {
            if (initAction != null)
                initAction(@this);
            return @this;
        }
    }
}

