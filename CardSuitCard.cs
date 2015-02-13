using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BladeAndSoulGossipCards
{
    class CardSuitCard
    {
        [Newtonsoft.Json.JsonProperty("編號")]
        public int No { get; set; }


        [Newtonsoft.Json.JsonProperty("屬性")]
        public PropertyValue[] Propertys { get; set; }

        [Newtonsoft.Json.JsonProperty("最大合成")]
        public int MaxAppreciation { get; set; }
    }
}
