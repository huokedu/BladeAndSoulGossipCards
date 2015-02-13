using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BladeAndSoulGossipCards
{
    class SetEffect
    {
        public string Id { get; set; }
        public int Count { get; set; }

        PropertyValue[] _PropertyValues;
        

        public SetEffect(PropertyValue[] property_value)
        {            
            _PropertyValues = property_value;
        }
        internal int GetValue(Property property)
        {
            return _PropertyValues.GetValue(property);
        }
    }
}
