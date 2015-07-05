using System;

namespace common_lib
{
    public static class FluentObjectInitialisationExtensions
    {        
        public static T Init<T>(this T @this, Action<T> initAction)
        {
            if (initAction != null)
                initAction(@this);
            return @this;
        }
    }
}

