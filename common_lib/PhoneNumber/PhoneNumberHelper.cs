using System;
using PhoneNumbers;

namespace common_lib
{
    public class PhoneNumberHelper : SingletonBase<PhoneNumberHelper>
    {
        public string get_2L_country_code_for_valid_international_number(string input)
        {
            try
            {
                var number_proto = phoneUtil.Parse(input, string.Empty);            
                return phoneUtil.GetRegionCodeForNumber(number_proto);
            }
            catch (NumberParseException e)
            {
                //Log exception somehow
                throw;
            }
        }

        private PhoneNumberHelper()
        {
            phoneUtil = PhoneNumberUtil.GetInstance();

        }

        private readonly PhoneNumberUtil phoneUtil;
    }
}

