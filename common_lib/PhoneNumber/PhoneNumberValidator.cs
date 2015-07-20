using System;
using PhoneNumbers;

namespace common_lib
{
    public class PhoneNumberValidator : SingletonBase<PhoneNumberValidator>
    {
        public bool is_valid_number(string input)
        {
            try
            {
                var number_proto = phoneUtil.Parse(input, string.Empty);

                return phoneUtil.IsValidNumber(number_proto);
            }
            catch (NumberParseException e)
            {
                //Log exception somehow
                return false;
            }
        }

        private PhoneNumberValidator()
        {
            phoneUtil = PhoneNumberUtil.GetInstance();
        }

        private readonly PhoneNumberUtil phoneUtil;
    }
}

