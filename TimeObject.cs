using System;
using System.Collections.Generic;
using System.Text;

namespace ExampleTelegramBot
{
    class TimeObject
    {
        public string abbreviation { get; set; }
        public string client_ip { get; set; }
        public string datetime { get; set; }
        public string day_of_week { get; set; }
        public string day_of_year { get; set; }
        public string dst { get; set; }
        public string dst_from { get; set; }
        public string dst_offset { get; set; }
        public string dst_until { get; set; }
        public string raw_offset { get; set; }
        public string timezone { get; set; }
        public string unixtime { get; set; }
        public string utc_datetime { get; set; }
        public string utc_offset { get; set; }
        public string week_number { get; set; }
    }
}
