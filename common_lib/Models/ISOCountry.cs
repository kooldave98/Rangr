using System;

namespace common_lib
{
    public class ISOCountry
    {
        public string ISOName { get; private set;}
        public string ISODialCode { get; private set;}
        public string ISO2LNameCode { get; private set;}

        public ISOCountry(string iso_name, string iso_dial_code, string iso_2l_name_code)
        {
            ISOName = Guard.IsNotNull(iso_name, "iso_name");
            ISODialCode = Guard.IsNotNull(iso_dial_code, "iso_dial_code");
            ISO2LNameCode = Guard.IsNotNull(iso_2l_name_code, "iso_2l_name_code");
        }
    }
}

