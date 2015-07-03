using System;
using ios_ui_lib;
using common_lib;

namespace experiments.ios
{
    public class SequenceViewModel : MobileEntrySequenceViewModel
    {
        public override string format_input(string input)
        {            
            return formatter.format_number(input);
        }

        public override bool is_valid_international_number(string input)
        {
            return validator.is_valid_number(input);
        }

        public override ISOCountry[] iso_countries { get; protected set;}


        public SequenceViewModel()
        {
            iso_countries = new GetCountryCodes().execute().ToArray();
            validator = new PhoneNumberValidator();
            formatter = new PhoneNumberFormatter();
        }

        private PhoneNumberValidator validator;
        private PhoneNumberFormatter formatter;
    }
}

