using System;
using System.Collections.Generic;
using solid_lib;

namespace App.Common
{
    public class GetCountryCodes
    {
        public List<CountryCallingCode> execute()
        {
            return Guard.IsNotNull(codes, "codes");
        }

        private List<CountryCallingCode> codes = new List<CountryCallingCode>()
        {
            new CountryCallingCode()
            {
                identifier = "United Kingdom",
                value = "+44"
            },
            new CountryCallingCode()
            {
                identifier = "Nigeria",
                value = "+234",
            },
        };
    }

    public class CountryCallingCode
    {
        public string identifier { get; set; }

        public string value { get; set; }
    }
}

