using System;
using System.Collections.Generic;

namespace common_lib
{
    public class GetCountryCodes
    {
        public List<ISOCountry> execute()
        {
            return Guard.IsNotNull(codes, "codes");
        }

        private List<ISOCountry> codes = new List<ISOCountry>()
        {                
            new ISOCountry("Nigeria", "+234", "NG"),
            new ISOCountry("United Kingdom", "+44", "GB"),
            new ISOCountry("United States", "+1", "US")                    
        };
    }
}

