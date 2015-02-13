using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BladeAndSoulGossipCards
{
    class PropertyValue : Property
    {
        [Newtonsoft.Json.JsonProperty("數值")]
        public int Value;
    }

    static class PropertyValueExt
    {
        public static int GetValue(this PropertyValue[] pvs, Property property)
        {
            return (from pv in pvs where pv.Id == property.Id select pv.Value).Sum();
        }
    }
}
