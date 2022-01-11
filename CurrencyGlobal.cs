using System;
using System.Collections.Generic;
using System.Text;

namespace ExampleTelegramBot
{
    public class CurrencyGlobal
    {
        public bool success { get; set; }
        public long timestamp { get; set; }
        public string date { get; set; }
        public CurrencyObject rates { get; set; }
    }
}
