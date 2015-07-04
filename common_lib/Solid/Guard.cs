using System;

namespace common_lib
{
    public static class Guard
    {
        public static T IsNotNull<T>(T arg, string name)
        {
            if (arg == null)
            {
                throw new ArgumentNullException(name);
            }

            return arg;
        }
    }
}

