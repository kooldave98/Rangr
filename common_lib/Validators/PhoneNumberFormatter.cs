using System;
using PhoneNumbers;

namespace common_lib
{
    public class PhoneNumberFormatter
    {
        public string format_number(string input)
        {
            try
            {
                var number_proto = phoneUtil.Parse(input, string.Empty);            

                return phoneUtil.Format(number_proto, PhoneNumberFormat.INTERNATIONAL);

            }
            catch (NumberParseException e)
            {
                //Log exception somehow
                return input;
            }
        }

        public PhoneNumberFormatter()
        {
            phoneUtil = PhoneNumberUtil.GetInstance();
        }

        private readonly PhoneNumberUtil phoneUtil;
    }
}

