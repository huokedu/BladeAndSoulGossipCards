using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BladeAndSoulGossipCards
{
    class CardSuitEffect
    {
        [Newtonsoft.Json.JsonProperty("數量")]
        public int Count { get; set; }

        [Newtonsoft.Json.JsonProperty("屬性")]
        public PropertyValue[] Propertys { get; set; }
    }
}
