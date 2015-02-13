using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BladeAndSoulGossipCards
{
    class CardSuit
    {
        [Newtonsoft.Json.JsonProperty("八卦牌")]
        public string Name { get; set; }

        [Newtonsoft.Json.JsonProperty("套裝效果")]
        public CardSuitEffect[] Effects { get; set; }

        [Newtonsoft.Json.JsonProperty("牌組")]
        public CardSuitCard[] Cards { get; set; }
    }
}
