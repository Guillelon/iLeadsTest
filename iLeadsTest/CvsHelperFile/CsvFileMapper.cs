using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iLeadsTest
{
    public class CsvFileMapper
    {
        public string PinNumberProperty { get; set; }
        public string FirstNameProperty { get; set; }
        public string LastNameProperty { get; set; }
        public string StreetAddressProperty { get; set; }
        public string CityProperty { get; set; }
        public string StateProperty { get; set; }
        public string ZipCodeProperty { get; set; }
    }

    public sealed class CsvFileMappersMap : CsvClassMap<CsvFileMapper>
    {
        public CsvFileMappersMap(string hola)
        {
            Map(m => m.PinNumberProperty).Name("PIN Number");
            Map(m => m.FirstNameProperty).Name("First Name");
            Map(m => m.LastNameProperty).Name("Last Name");
            Map(m => m.StreetAddressProperty).Name("Street Address");
            Map(m => m.CityProperty).Name("City");
            Map(m => m.StateProperty).Name("State");
            Map(m => m.ZipCodeProperty).Name("Zip");
        }
    }
}