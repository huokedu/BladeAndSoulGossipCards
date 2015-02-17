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

        public static PropertyValue Build(string type , string value)
        {
            var id = _ToType(type);
            int val = _Parse(value);
            return new PropertyValue { Id = id, Value = val };
        }

        private static int _Parse(string value)
        {
            int val = int.Parse(value);
            
            return val;
        }

        private static PROPERTY_TYPE _ToType(string type)
        {

            PROPERTY_TYPE pt = (PROPERTY_TYPE)Enum.Parse(typeof(PROPERTY_TYPE), type, true);
            return pt;
        }
    }

    static class PropertyValueExt
    {
        public static int GetValue(this PropertyValue[] pvs, Property property)
        {
            return (from pv in pvs where pv.Id == property.Id select pv.Value).Sum();
        }

        
    }
}
