using System;
using solid_lib;
using System.Collections.Generic;
using System.Linq;

namespace ios_ui_lib
{
    public abstract class MobileEntrySequenceViewModel : ISelectableCountries, NumberEntryCell.IPhoneNumberFormatter
    {
        public abstract bool is_valid_international_number(string input);

        public abstract string format_input(string input);

        public abstract ISOCountry[] iso_countries { get; protected set;}

        public int selected_country_index { get; private set;}

        public ISOCountry selected_country{ 
            get{ return iso_countries[selected_country_index]; }
        }

        public void select_country_index(int country)
        {
            selected_country_index = country;
        }

        public string mobile_number { get; set;}
    }

    public interface ISelectableCountries : ISelectedCountry
    {
        ISOCountry[] iso_countries { get; }
    }

    public interface ISelectedCountry
    {
        int selected_country_index { get;}

        ISOCountry selected_country { get;}
    }


}

