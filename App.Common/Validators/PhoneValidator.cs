using System;
using PhoneNumbers;

namespace App.Common
{
    public class PhoneNumberValidator
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

        public PhoneNumberValidator()
        {
            phoneUtil = PhoneNumberUtil.GetInstance();
        }

        private readonly PhoneNumberUtil phoneUtil;
    }
}

