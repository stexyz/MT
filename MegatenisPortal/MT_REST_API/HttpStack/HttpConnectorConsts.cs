using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MT_REST_API.HttpStack
{
    public enum ReportCategory
    {
        EshopFinished,
        StoreTotal,
        DirectTotal,
        OwnStuff,
        Returned,
        ToBeSolved
    }

    public static partial class HttpConnector
    {
        private static string connectionString = "action=login&login=megatenis&heslo=Megaspawn22";
        private static string loginUri = "http://www.megatenis.cz/admin/";
        private static string CsvUri = "http://www.megatenis.cz/admin/objednavky-export/";

        private static string PurchUri = "http://www.megatenis.cz/admin/export-zbozi-beta/";

        private static string purchasePricePayload =
            "classification%5B%5D=standard&classification%5B%5D=hidden&classification%5B%5D=blocked&column%5B%5D=catalogue_number&column%5B%5D=purchase_price&rezim_obrazku=zadne&id_mapa%5B%5D=-1&id_mapa%5B%5D=197&id_mapa%5B%5D=200&id_mapa%5B%5D=201&id_mapa%5B%5D=204&id_mapa%5B%5D=203&id_mapa%5B%5D=109&id_mapa%5B%5D=195&id_mapa%5B%5D=118&id_mapa%5B%5D=146&id_mapa%5B%5D=145&id_mapa%5B%5D=147&id_mapa%5B%5D=119&id_mapa%5B%5D=158&id_mapa%5B%5D=152&id_mapa%5B%5D=164&id_mapa%5B%5D=165&id_mapa%5B%5D=166&id_mapa%5B%5D=187&id_mapa%5B%5D=199&id_mapa%5B%5D=160&id_mapa%5B%5D=175&id_mapa%5B%5D=189&id_mapa%5B%5D=190&id_mapa%5B%5D=191&id_mapa%5B%5D=176&id_mapa%5B%5D=192&id_mapa%5B%5D=194&id_mapa%5B%5D=193&id_mapa%5B%5D=196&id_mapa%5B%5D=177&id_mapa%5B%5D=178&id_mapa%5B%5D=208&id_mapa%5B%5D=217&id_mapa%5B%5D=206&id_mapa%5B%5D=179&id_mapa%5B%5D=180&id_mapa%5B%5D=181&id_mapa%5B%5D=207&id_mapa%5B%5D=155&id_mapa%5B%5D=157&id_mapa%5B%5D=156&id_mapa%5B%5D=110&id_mapa%5B%5D=182&id_mapa%5B%5D=183&id_mapa%5B%5D=184&id_mapa%5B%5D=167&id_mapa%5B%5D=170&id_mapa%5B%5D=169&id_mapa%5B%5D=168&id_mapa%5B%5D=218&id_mapa%5B%5D=153&id_mapa%5B%5D=213&id_mapa%5B%5D=209&id_mapa%5B%5D=188&id_mapa%5B%5D=219&id_mapa%5B%5D=186&id_mapa%5B%5D=162&id_mapa%5B%5D=171&id_mapa%5B%5D=172&id_mapa%5B%5D=220&id_mapa%5B%5D=210&id_mapa%5B%5D=211&id_mapa%5B%5D=212&id_mapa%5B%5D=198&id_vyrobce%5B%5D=0&id_vyrobce%5B%5D=5&id_vyrobce%5B%5D=6&id_vyrobce%5B%5D=7&id_vyrobce%5B%5D=4&id_vyrobce%5B%5D=8&id_vyrobce%5B%5D=9&id_vyrobce%5B%5D=3&id_dostupnosti%5B%5D=-1&id_dostupnosti%5B%5D=1&id_dostupnosti%5B%5D=6&id_dostupnosti%5B%5D=2&id_dostupnosti%5B%5D=4&x=15&y=11";

        // {0}:01.01.2013, {1}:31.01.2013
        private static string EshopFinishedPayload =
            "action=csv&delimiter=%3B&datum_od={0}&datum_do={1}&hlavicka=ano&sloucit_prijmeni_jmeno=ano&kopirovat_adresy=ano&export_type=normal&export_table=detaily&stav%5B1%5D=1&x=20&y=9";

        private static string DirectTotalPayload =
            "action=csv&delimiter=%3B&datum_od={0}&datum_do={1}&hlavicka=ano&sloucit_prijmeni_jmeno=ano&kopirovat_adresy=ano&export_type=normal&export_table=detaily&stav%5B16%5D=16&stav%5B15%5D=15&x=66&y=12";

        private static string OwnStuffPayload =
            "action=csv&delimiter=%3B&datum_od={0}&datum_do={1}&hlavicka=ano&sloucit_prijmeni_jmeno=ano&kopirovat_adresy=ano&export_type=normal&export_table=detaily&stav%5B14%5D=14&stav%5B17%5D=17&x=40&y=9";

        private static string ReturnedPayload =
            "action=csv&delimiter=%3B&datum_od={0}&datum_do={1}&hlavicka=ano&sloucit_prijmeni_jmeno=ano&kopirovat_adresy=ano&export_type=normal&export_table=detaily&stav%5B19%5D=19&stav%5B2%5D=2&stav%5B10%5D=10&x=23&y=17";

        private static string StoreTotalPayload =
            "action=csv&delimiter=%3B&datum_od={0}&datum_do={1}&hlavicka=ano&sloucit_prijmeni_jmeno=ano&kopirovat_adresy=ano&export_type=normal&export_table=detaily&stav%5B7%5D=7&stav%5B13%5D=13&x=70&y=20";

        private static string ToBeSolvedPayload =
            "action=csv&delimiter=%3B&datum_od={0}&datum_do={1}&hlavicka=ano&sloucit_prijmeni_jmeno=ano&kopirovat_adresy=ano&export_type=normal&export_table=detaily&stav%5B11%5D=11&stav%5B6%5D=6&stav%5B3%5D=3&stav%5B4%5D=4&stav%5B5%5D=5&stav%5B18%5D=18&x=51&y=29";

        private static Uri baseUri = new Uri("http://www.megatenis.cz");

        private static string getDateTimeFormated(DateTime dateTime)
        {
            return string.Format("{0}.{1}.{2}", dateTime.Day, dateTime.Month, dateTime.Year);
        }

        private static string GetPayloadForOrdersCsv(DateTime from, DateTime to, ReportCategory category)
        {
            if (from > to)
            {
                throw new ArgumentException(
                    string.Format(
                        "Invalid time interval: >from< argument ({0}) happens after the >to< argument ({1}).", from, to));
            }

            switch (category)
            {
                case ReportCategory.EshopFinished:
                    return string.Format(EshopFinishedPayload, getDateTimeFormated(from), getDateTimeFormated(to));
                case ReportCategory.DirectTotal:
                    return string.Format(DirectTotalPayload, getDateTimeFormated(from), getDateTimeFormated(to));
                case ReportCategory.OwnStuff:
                    return string.Format(OwnStuffPayload, getDateTimeFormated(from), getDateTimeFormated(to));
                case ReportCategory.Returned:
                    return string.Format(ReturnedPayload, getDateTimeFormated(from), getDateTimeFormated(to));
                case ReportCategory.StoreTotal:
                    return string.Format(StoreTotalPayload, getDateTimeFormated(from), getDateTimeFormated(to));
                case ReportCategory.ToBeSolved:
                    return string.Format(ToBeSolvedPayload, getDateTimeFormated(from), getDateTimeFormated(to));
            }
            return null;
        }

    }
}
