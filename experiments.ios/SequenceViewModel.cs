using System;
using ios_ui_lib;
using PhoneNumbers;

namespace experiments.ios
{
    public class SequenceViewModel : MobileEntrySequenceViewModel
    {
        public override string format_input(string input)
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

        public override bool is_valid_international_number(string input)
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

        public override ISOCountry[] iso_countries { get; protected set;}


        public SequenceViewModel()
        {
            phoneUtil = PhoneNumberUtil.GetInstance();

            iso_countries = new ISOCountry[]
            {
                new ISOCountry("Nigeria", "+234", "NG"),
                new ISOCountry("United Kingdom", "+44", "GB"),
                new ISOCountry("United States", "+1", "US")
            };
        }

        private readonly PhoneNumberUtil phoneUtil;
        
    }
}

