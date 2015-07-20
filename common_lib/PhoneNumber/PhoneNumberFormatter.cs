using System;
using PhoneNumbers;

namespace common_lib
{
    public class PhoneNumberFormatter : SingletonBase<PhoneNumberFormatter>
    {
        public string format_number(string input, string default_ISO2LCountryCode = null)
        {
            return base_format_number(PhoneNumberFormat.INTERNATIONAL, input, default_ISO2LCountryCode);
        }

        public string clean_number(string input, string default_ISO2LCountryCode = null)
        {
            return base_format_number(PhoneNumberFormat.E164, input, default_ISO2LCountryCode);
        }

        private string base_format_number(PhoneNumberFormat phone_number_format, string input, string default_ISO2LCountryCode = null)
        {
            default_ISO2LCountryCode = default_ISO2LCountryCode ?? string.Empty;

            try
            {
                var number_proto = phoneUtil.Parse(input, default_ISO2LCountryCode);            

                return phoneUtil.Format(number_proto, phone_number_format);

            }
            catch (NumberParseException e)
            {
                //Log exception somehow
                return input;
            }
        }

        private PhoneNumberFormatter()
        {
            phoneUtil = PhoneNumberUtil.GetInstance();
        }

        private readonly PhoneNumberUtil phoneUtil;
    }
}

