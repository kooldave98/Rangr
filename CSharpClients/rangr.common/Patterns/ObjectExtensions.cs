using System;

namespace rangr.common
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

